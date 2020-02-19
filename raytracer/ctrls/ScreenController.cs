using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace raytracer
{
	public class ScreenController
	{
		#region Events

		public event PaletteChangedEventHandler PaletteChanged;

		#endregion

		#region Constants

		public const int SCREEN_WIDTH = 256;
		public const int SCREEN_HEIGHT = 224;
		public const int BITS_PER_PIXEL = 8;

		#endregion

		#region Fields

		private BitmapPalette _palette;
		private Color[] _colors;
		private readonly byte[] _pixels;

		#endregion

		#region Constructors

		private ScreenController()
		{
			_palette = PaletteFactory.BuildMinicraftPalette();
			if (_palette.Colors.Count > 256)
			{
				throw new Exception("There are too many colors in the palette.");
			}

			RebuildBitmap();

			_colors = _palette.Colors.ToArray();
			_pixels = new byte[SCREEN_WIDTH * SCREEN_HEIGHT * BITS_PER_PIXEL / 8];
			//GenerateStatic();
			//Fill((byte)(192));
			//Refresh();
		}

		#endregion

		#region Properties

		public static ScreenController Instance { get; private set; }

		public WriteableBitmap Bitmap { get; private set; }

		public BitmapPalette Palette
		{
			get
			{
				return _palette;
			}
			set
			{
				if (_palette != value)
				{
					_palette = value;
					PaletteChanged?.Invoke(this, new PaletteChangedEventArgs(_palette));
				}
			}
		}

		public byte[] Pixels
		{
			get
			{
				return _pixels;
			}
		}

		public int Stride
		{
			get
			{
				return (SCREEN_WIDTH * BITS_PER_PIXEL + 7) / 8;
			}
		}

		#endregion

		#region Methods

		public static ScreenController Initialize()
		{
			Instance = new ScreenController();
			return Instance;
		}

		public void Refresh()
		{
			if (!App.Current.Dispatcher.CheckAccess())
			{
				App.Current.Dispatcher.Invoke(Refresh);
			}
			else
			{
				Bitmap.WritePixels(new Int32Rect(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Pixels, Stride, 0);
			}
		}

		public void GenerateStatic()
		{
			var rand = new Random();
			for (var n = 0; n < _pixels.Length; n++)
			{
				_pixels[n] = (byte)rand.Next(256);
			}
		}

		public void Fill(byte color)
		{
			for (var n = 0; n < _pixels.Length; n++)
			{
				_pixels[n] = color;
			}
		}

		public void SetPixel(int index, byte color)
		{
			if ((index >= 0) && (index < _pixels.Length))
			{
				_pixels[index] = color;
			}
		}

		public void SetPixel(int x, int y, byte color)
		{
			if ((x < 0) || (x >= SCREEN_WIDTH) || (y < 0) || (y >= SCREEN_HEIGHT))
			{
				return;
			}
			SetPixel(x + y * Stride, color);
		}

		public byte GetPixel(int index)
		{
			if ((index >= 0) && (index < _pixels.Length))
			{
				return _pixels[index];
			}
			return 0;
		}

		public byte GetPixel(int x, int y)
		{
			if ((x < 0) || (x >= SCREEN_WIDTH) || (y < 0) || (y >= SCREEN_HEIGHT))
			{
				return 0;
			}
			return GetPixel(x + y * Stride);
		}

		public void BlitHLine(int x1, int x2, int y, byte color)
		{
			for (var x = x1; x <= x2; x++)
			{
				SetPixel(x, y, color);
			}
		}

		public void BlitVLine(int x, int y1, int y2, byte color)
		{
			for (var y = y1; y <= y2; y++)
			{
				SetPixel(x, y, color);
			}
		}

		public void BlitRect(int x1, int x2, int y1, int y2, byte color)
		{
			BlitHLine(x1, x2, y1, color);
			BlitHLine(x1, x2, y2, color);
			BlitVLine(x1, y1 + 1, y2 - 1, color);
			BlitVLine(x2, y1 + 1, y2 - 1, color);
		}

		public void FillRect(int x1, int x2, int y1, int y2, byte color)
		{
			for (var y = y1; y <= y2; y++)
			{
				for (var x = x1; x <= x2; x++)
				{
					SetPixel(x, y, color);
				}
			}
		}

		public void BlitChar(int x, int y, byte foregroundColor, byte backgroundColor, char ch)
		{
			var charValue = OEM437.DATA[ch];
			for (var yc = 0; yc < OEM437.CHAR_HEIGHT; yc++)
			{
				for (var xc = 0; xc < OEM437.CHAR_WIDTH; xc++, charValue = charValue >> 1)
				{
					if ((charValue & 1) != 0)
					{
						SetPixel(x + xc, y + OEM437.CHAR_HEIGHT - yc - 1, foregroundColor);
					}
					else if (backgroundColor >= 0)
					{
						SetPixel(x + xc, y + OEM437.CHAR_HEIGHT - yc - 1, backgroundColor);
					}
				}
			}
		}

		public void BlitChar(int x, int y, byte foregroundColor, char ch)
		{
			var charValue = OEM437.DATA[ch];
			for (var yc = 0; yc < OEM437.CHAR_HEIGHT; yc++)
			{
				for (var xc = 0; xc < OEM437.CHAR_WIDTH; xc++, charValue = charValue >> 1)
				{
					if ((charValue & 1) != 0)
					{
						SetPixel(x + xc, y + OEM437.CHAR_HEIGHT - yc - 1, foregroundColor);
					}
				}
			}
		}

		public void BlitChar2x(int x, int y, byte foregroundColor, char ch)
		{
			var charValue = OEM437.DATA[ch];
			for (var yc = 0; yc < OEM437.CHAR_HEIGHT; yc++)
			{
				for (var xc = 0; xc < OEM437.CHAR_WIDTH; xc++, charValue = charValue >> 1)
				{
					if ((charValue & 1) != 0)
					{
						SetPixel(x + xc * 2 + 0, y + (OEM437.CHAR_HEIGHT - yc - 1) * 2 + 0, foregroundColor);
						SetPixel(x + xc * 2 + 0, y + (OEM437.CHAR_HEIGHT - yc - 1) * 2 + 1, foregroundColor);
						SetPixel(x + xc * 2 + 1, y + (OEM437.CHAR_HEIGHT - yc - 1) * 2 + 0, foregroundColor);
						SetPixel(x + xc * 2 + 1, y + (OEM437.CHAR_HEIGHT - yc - 1) * 2 + 1, foregroundColor);
					}
				}
			}
		}

		public void BlitString(int x, int y, byte foregroundColor, byte backgroundColor, string text)
		{
			foreach (var ch in text)
			{
				BlitChar(x, y, foregroundColor, backgroundColor, ch);
				x += OEM437.CHAR_WIDTH;
			}
		}

		public void BlitString(int x, int y, byte foregroundColor, string text)
		{
			foreach (var ch in text)
			{
				BlitChar(x, y, foregroundColor, ch);
				x += OEM437.CHAR_WIDTH;
			}
		}

		public void BlitString2x(int x, int y, byte foregroundColor, string text)
		{
			foreach (var ch in text)
			{
				BlitChar2x(x, y, foregroundColor, ch);
				x += OEM437.CHAR_WIDTH * 2;
			}
		}

		public void RebuildBitmap()
		{
			Bitmap = new WriteableBitmap(BitmapSource.Create(SCREEN_WIDTH, SCREEN_HEIGHT, 96, 96, PixelFormats.Indexed8, Palette, new byte[Stride * SCREEN_HEIGHT], Stride));
		}

		#endregion

		#region Event Handlers

		internal void OnPaletteChanged(object sender, PaletteIndexChangedEventArgs e)
		{
			_colors[e.Index] = e.Color;
			Palette = new BitmapPalette(_colors);
		}

		#endregion
	}
}
