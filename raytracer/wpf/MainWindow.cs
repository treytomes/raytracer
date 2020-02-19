using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace raytracer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public class MainWindow : Window
	{
		#region Constants

		private const int SCALE = 3;

		#endregion

		#region Fields

		private Image _screenImage;
		private Image _overlayImage;

		#endregion

		#region Constructors

		public MainWindow()
		{
			Title = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title; // "pong";
			ResizeMode = ResizeMode.NoResize;
			SizeToContent = SizeToContent.WidthAndHeight;

			_screenImage = new Image()
			{
				Width = ScreenController.SCREEN_WIDTH,
				Height = ScreenController.SCREEN_HEIGHT,
				SnapsToDevicePixels = true
			};

			//_screenImage.Effect = new System.Windows.Media.Effects.BlurEffect()
			//{
			//	RenderingBias = System.Windows.Media.Effects.RenderingBias.Quality,
			//	Radius = SCALE,
			//	KernelType = System.Windows.Media.Effects.KernelType.Box
			//};

			_screenImage.Effect = new System.Windows.Media.Effects.DropShadowEffect()
			{
				BlurRadius = 30,
				Color = Colors.White,
				Opacity = 1,
				ShadowDepth = 0
			};

			RenderOptions.SetBitmapScalingMode(_screenImage, BitmapScalingMode.NearestNeighbor);

			_overlayImage = new Image()
			{
				Width = ScreenController.SCREEN_WIDTH,
				Height = ScreenController.SCREEN_HEIGHT,
				SnapsToDevicePixels = true
			};
			RenderOptions.SetBitmapScalingMode(_overlayImage, BitmapScalingMode.NearestNeighbor);

			var grid = new Grid();
			grid.Children.Add(_screenImage);
			grid.Children.Add(_overlayImage);

			Content = grid;

			Closing += MainWindow_Closing;
			Initialized += MainWindow_Initialized;
			IsKeyboardFocusWithinChanged += MainWindow_IsKeyboardFocusWithinChanged;
			Loaded += MainWindow_Loaded;
			PreviewKeyDown += MainWindow_PreviewKeyDown;
			PreviewKeyUp += MainWindow_PreviewKeyUp;
		}

		#endregion

		#region Methods

		private void RebuildBitmap()
		{
			if (!Application.Current.Dispatcher.CheckAccess())
			{
				App.Current.Dispatcher.Invoke(RebuildBitmap);
			}
			else
			{
				ScreenController.Instance.RebuildBitmap();
				_screenImage.Source = ScreenController.Instance.Bitmap;
			}
		}

		private void BuildOverlay3x()
		{
			const int BYTES_PER_PIXEL = 4;
			const int STRIDE = ScreenController.SCREEN_WIDTH * SCALE * BYTES_PER_PIXEL;

			const int WIDTH = ScreenController.SCREEN_WIDTH * SCALE;
			const int HEIGHT = ScreenController.SCREEN_HEIGHT * SCALE;

			var overlay3x = new WriteableBitmap(WIDTH, HEIGHT, 96, 96, PixelFormats.Bgra32, null);

			var pixels = new uint[WIDTH * HEIGHT];
			for (var y = 0; y < ScreenController.SCREEN_HEIGHT * SCALE; y += SCALE)
			{
				for (var x = 0; x < ScreenController.SCREEN_WIDTH * SCALE; x += SCALE)
				{
					pixels[(y * WIDTH) + x + 0] = 0x2CF31C00;
					pixels[(y * WIDTH) + x + 1] = 0x1500FFCE;
					pixels[(y * WIDTH) + x + 2] = 0x1C1200A3;

					pixels[(y * WIDTH) + x + 0] = 0x557E0F00;
					pixels[(y * WIDTH) + x + 1] = 0x40004F43;
					pixels[(y * WIDTH) + x + 2] = 0x45070067;

					pixels[(y * WIDTH) + x + 0] = 0x800B0100;
					pixels[(y * WIDTH) + x + 1] = 0x8B000907;
					pixels[(y * WIDTH) + x + 2] = 0x8D01000C;
				}
			}
			overlay3x.WritePixels(new Int32Rect(0, 0, WIDTH, HEIGHT), pixels, STRIDE, 0);

			_overlayImage.Source = overlay3x;
		}

		#endregion

		#region Event Handlers

		private void MainWindow_Initialized(object sender, EventArgs e)
		{
			GameController.Instance.State = GameController.LoopState.Running;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			ScreenController.Initialize();
			RebuildBitmap();
			ScreenController.Instance.PaletteChanged += OnPaletteChanged;

			GameController.Initialize();

			_screenImage.Width = _overlayImage.Width = ScreenController.Instance.Bitmap.PixelWidth * SCALE;
			_screenImage.Height = _overlayImage.Height = ScreenController.Instance.Bitmap.PixelHeight * SCALE;

			BuildOverlay3x();
		}

		private void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			GameController.Instance.Dispose();
			App.Current.Shutdown(0);
		}

		private void MainWindow_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (GameController.Instance != null)
			{
				GameController.Instance.State = IsKeyboardFocusWithin ? GameController.LoopState.Running : GameController.LoopState.Paused;
			}
		}

		private void OnPaletteChanged(object sender, PaletteChangedEventArgs e)
		{
			RebuildBitmap();
		}

		private void MainWindow_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			GameController.Instance.HandleEvent(e);
		}

		private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			GameController.Instance.HandleEvent(e);
		}

		#endregion
	}
}
