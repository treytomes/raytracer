using System;
using System.Windows.Media;

namespace raytracer
{
	public class PaletteIndexChangedEventArgs : EventArgs
	{
		public PaletteIndexChangedEventArgs(int index, Color color)
			: base()
		{
			Index = index;
			Color = color;
		}

		public int Index { get; }
		public Color Color { get; }
	}

	public delegate void PaletteIndexChangedEventHandler(object sender, PaletteIndexChangedEventArgs e);
}
