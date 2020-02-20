using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace raytracer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public class MainWindow : Window
	{
		#region Fields

		private Image _screenImage;

		#endregion

		#region Constructors

		public MainWindow()
		{
			Title = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title; // "pong";
			ResizeMode = ResizeMode.NoResize;
			SizeToContent = SizeToContent.WidthAndHeight;

			ScreenController.Initialize();

			_screenImage = new Image()
			{
				Width = ScreenController.Instance.Width,
				Height = ScreenController.Instance.Height,
				SnapsToDevicePixels = true
			};

			RenderOptions.SetBitmapScalingMode(_screenImage, BitmapScalingMode.NearestNeighbor);

			var grid = new Grid();
			grid.Children.Add(_screenImage);

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

		#endregion

		#region Event Handlers

		private void MainWindow_Initialized(object sender, EventArgs e)
		{
			GameController.Instance.State = GameController.LoopState.Running;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			RebuildBitmap();

			GameController.Initialize();

			_screenImage.Width = ScreenController.Instance.Bitmap.PixelWidth;
			_screenImage.Height = ScreenController.Instance.Bitmap.PixelHeight;
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
