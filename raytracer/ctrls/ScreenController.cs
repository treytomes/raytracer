using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace raytracer
{
	public class ScreenController
	{
		#region Constants

		public const int BITS_PER_PIXEL = 24;
		public const int BYTES_PER_PIXEL = BITS_PER_PIXEL / 8;
		private const int DEFAULT_WIDTH = 256 * 3;
		private const int DEFAULT_HEIGHT = 224 * 3;

		#endregion

		#region Fields

		public readonly int Width;
		public readonly int Height;

		private readonly int _stride;
		private readonly byte[] _pixels;

		#endregion

		#region Constructors

		private ScreenController(int width, int height)
		{
			Width = width;
			Height = height;
			_stride = Width * BYTES_PER_PIXEL;

			RebuildBitmap();

			_pixels = new byte[_stride * Height];
		}

		#endregion

		#region Properties

		public static ScreenController Instance { get; private set; }

		public WriteableBitmap Bitmap { get; private set; }

		public byte[] Pixels
		{
			get
			{
				return _pixels;
			}
		}

		#endregion

		#region Methods

		public static ScreenController Initialize(int width = DEFAULT_WIDTH, int height = DEFAULT_HEIGHT)
		{
			Instance = new ScreenController(width, height);
			return Instance;
		}

		/// <summary>
		/// Save the currently displayed image as a png.
		/// </summary>
		public void Save(string path)
		{
			using (var stream = new FileStream(path, FileMode.Create))
			{
				var encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(Bitmap));
				encoder.Save(stream);
			}
		}

		public void Refresh()
		{
			if ((App.Current != null) && !App.Current.Dispatcher.CheckAccess())
			{
				App.Current.Dispatcher.Invoke(Refresh);
			}
			else
			{
				Bitmap.WritePixels(new Int32Rect(0, 0, Width, Height), Pixels, _stride, 0);
			}
		}

		public void SetPixel(int x, int y, math.Color color)
		{
			var index = y * _stride + x * BYTES_PER_PIXEL;
			_pixels[index++] = (byte)(color.Red * 255);
			_pixels[index++] = (byte)(color.Green * 255);
			_pixels[index] = (byte)(color.Blue * 255);
		}

		public math.Color GetPixel(int x, int y)
		{
			var index = y * _stride + x * BYTES_PER_PIXEL;
			var red = _pixels[index++];
			var green = _pixels[index++];
			var blue = _pixels[index];
			return new math.Color(red / 255.0f, green / 255.0f, blue / 255.0f);
		}

		public void RebuildBitmap()
		{
			Bitmap = new WriteableBitmap(BitmapSource.Create(Width, Height, 96, 96, PixelFormats.Rgb24, null, new byte[_stride * Height], _stride));
		}

		#endregion
	}
}
