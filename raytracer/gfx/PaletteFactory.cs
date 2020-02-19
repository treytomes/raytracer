using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace raytracer
{
	public static class PaletteFactory
	{
		public static BitmapPalette BuildGrayscalePalette()
		{
			var colors = new List<Color>();
			for (var n = 0; n < 256; n++)
			{
				colors.Add(Color.FromRgb((byte)n, (byte)n, (byte)n));
			}

			return new BitmapPalette(colors);
		}

		public static BitmapPalette BuildMinicraftPalette()
		{
			const int BITS = 6;
			var colors = new List<Color>();
			for (var r = 0; r < BITS; r++)
			{
				for (var g = 0; g < BITS; g++)
				{
					for (var b = 0; b < BITS; b++)
					{
						var rr = r * 255 / (BITS - 1);
						var gg = g * 255 / (BITS - 1);
						var bb = b * 255 / (BITS - 1);

						var mid = (rr * 30 + gg * 59 + bb * 11) / 100;

						var r1 = ~~(((rr + mid * 1) / 2) * 230 / 255 + 10);
						var g1 = ~~(((gg + mid * 1) / 2) * 230 / 255 + 10);
						var b1 = ~~(((bb + mid * 1) / 2) * 230 / 255 + 10);

						colors.Add(Color.FromRgb((byte)r1, (byte)g1, (byte)b1));
					}
				}
			}

			while (colors.Count < 256)
			{
				colors.Add(Colors.Black);
			}

			//var colors = new Color[256];
			//for (var n = 0; n < 256; n++)
			//{
			//	var red = (int)(n / 100) % 10;
			//	var green = (int)(n / 10) % 10;
			//	var blue = n % 10;
			//	colors[n] = Color.FromRgb((byte)(red * 36), (byte)(green * 6), (byte)blue);
			//}
			return new BitmapPalette(colors);
		}

		/// <remarks>https://github.com/geoffb/dawnbringer-palettes</remarks>
		public static BitmapPalette BuildDawnBringer8Palette()
		{
			var colors = new List<Color>()
			{
				Color.FromRgb(0x00, 0x00, 0x00), // black
				Color.FromRgb(0x55, 0x41, 0x5F), // violet
				Color.FromRgb(0x64, 0x69, 0x64), // gray
				Color.FromRgb(0xD7, 0x73, 0x55), // red
				Color.FromRgb(0x50, 0x8C, 0xD7), // blue
				Color.FromRgb(0x64, 0xB9, 0x64), // green
				Color.FromRgb(0xE6, 0xC8, 0x6E), // yellow
				Color.FromRgb(0xDC, 0xF5, 0xFF), // white
			};
			colors.AddRange(Enumerable.Range(0, 256 - colors.Count).Select(x => Colors.Black));
			return new BitmapPalette(colors);
		}

		/// <remarks>https://github.com/geoffb/dawnbringer-palettes</remarks>
		public static BitmapPalette BuildDawnBringer16Palette()
		{
			return new BitmapPalette(new List<Color>()
			{
				Color.FromRgb(0x14, 0x0C, 0x1C), // black
				Color.FromRgb(0x44, 0x24, 0x34), // dark red
				Color.FromRgb(0x30, 0x34, 0x6D), // dark blue
				Color.FromRgb(0x4E, 0x4A, 0x4F), // dark gray
				Color.FromRgb(0x85, 0x4C, 0x30), // brown
				Color.FromRgb(0x34, 0x65, 0x24), // dark green
				Color.FromRgb(0xD0, 0x46, 0x48), // red
				Color.FromRgb(0x75, 0x71, 0x61), // light gray
				Color.FromRgb(0x59, 0x7D, 0xCE), // light blue
				Color.FromRgb(0xD2, 0x7D, 0x2C), // orange
				Color.FromRgb(0x85, 0x95, 0xA1), // blue/gray
				Color.FromRgb(0x6D, 0xAA, 0x2C), // light green
				Color.FromRgb(0xD2, 0xAA, 0x99), // peach
				Color.FromRgb(0x6D, 0xC2, 0xCA), // cyan
				Color.FromRgb(0xDA, 0xD4, 0x5E), // yellow
				Color.FromRgb(0xDE, 0xEE, 0xD6), // white
			});
		}

		/// <remarks>https://github.com/geoffb/dawnbringer-palettes</remarks>
		public static BitmapPalette BuildDawnBringer32Palette()
		{
			return new BitmapPalette(new List<Color>()
			{
			Color.FromRgb(0x00, 0x00, 0x00), // black
			Color.FromRgb(0x22, 0x20, 0x34), // balhalla
			Color.FromRgb(0x45, 0x28, 0x3C), // loulou
			Color.FromRgb(0x66, 0x39, 0x31), // oiled cedar
			Color.FromRgb(0x8F, 0x56, 0x3B), // rope
			Color.FromRgb(0xDF, 0x71, 0x26), // tahiti gold
			Color.FromRgb(0xD9, 0xA0, 0x66), // twine
			Color.FromRgb(0xEE, 0xC3, 0x9A), // pancho
			Color.FromRgb(0xFB, 0xF2, 0x36), // golden fizz
			Color.FromRgb(0x99, 0xE5, 0x50), // atlantis
			Color.FromRgb(0x6A, 0xBE, 0x30), // christi
			Color.FromRgb(0x37, 0x94, 0x6E), // elf green
			Color.FromRgb(0x4B, 0x69, 0x2F), // dell
			Color.FromRgb(0x52, 0x4B, 0x24), // verdigris
			Color.FromRgb(0x32, 0x3C, 0x39), // opal
			Color.FromRgb(0x3F, 0x3F, 0x74), // deep koamaru
			Color.FromRgb(0x30, 0x60, 0x82), // venice blue
			Color.FromRgb(0x5B, 0x6E, 0xE1), // royal blue
			Color.FromRgb(0x63, 0x9B, 0xFF), // cornflower
			Color.FromRgb(0x5F, 0xCD, 0xE4), // viking
			Color.FromRgb(0xCB, 0xDB, 0xFC), // light steel blue
			Color.FromRgb(0xFF, 0xFF, 0xFF), // white
			Color.FromRgb(0x9B, 0xAD, 0xB7), // heather
			Color.FromRgb(0x84, 0x7E, 0x87), // topaz
			Color.FromRgb(0x69, 0x6A, 0x6A), // dim gray
			Color.FromRgb(0x59, 0x56, 0x52), // smokey ash
			Color.FromRgb(0x76, 0x42, 0x8A), // clairvoyant
			Color.FromRgb(0xAC, 0x32, 0x32), // brown
			Color.FromRgb(0xD9, 0x57, 0x63), // mandy
			Color.FromRgb(0xD7, 0x7B, 0xBA), // plum
			Color.FromRgb(0x8F, 0x97, 0x4A), // rainforest
			Color.FromRgb(0x8A, 0x6F, 0x30), // stinger
		});
		}
	}
}
