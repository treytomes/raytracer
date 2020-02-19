using System;
using System.Windows.Media.Imaging;

namespace raytracer
{
	public class PaletteChangedEventArgs : EventArgs
	{
		public PaletteChangedEventArgs(BitmapPalette palette)
			: base()
		{
			Palette = palette;
		}

		public BitmapPalette Palette { get; }
	}

	public delegate void PaletteChangedEventHandler(object sender, PaletteChangedEventArgs e);
}
