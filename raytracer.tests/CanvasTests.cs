using raytracer.math;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;

namespace raytracer.tests
{
	public class CanvasTests
	{
		[Fact(DisplayName = "Creating a canvas.")]
		public void Can_create_a_canvas()
		{
			var c = ScreenController.Initialize(10, 20);
			Assert.Equal(10, c.Width);
			Assert.Equal(20, c.Height);

			for (var y = 0; y < c.Height; y++)
			{
				for (var x = 0; x < c.Width; x++)
				{
					Assert.Equal(new Color(0, 0, 0), c.GetPixel(x, y));
				}
			}
		}

		[Fact(DisplayName = "Writing pixels to a canvas.")]
		public void Can_write_pixels()
		{
			var c = ScreenController.Initialize(10, 20);
			var red = new Color(1, 0, 0);
			c.SetPixel(2, 3, red);
			Assert.Equal(red, c.GetPixel(2, 3));
		}

		[Fact(DisplayName = "Save a canvas to a file.")]
		public void Can_save_a_canvas()
		{
			var c = ScreenController.Initialize(256, 256);
			for (var y = 0; y < c.Height; y++)
			{
				for (var x = 0; x < c.Width; x++)
				{
					var color = new Color((x ^ y) / 255.0f, (x & y) / 255.0f, (x | y) / 255.0f);
					c.SetPixel(x, y, color);
				}
			}
			c.Refresh();
			c.Save("test.png");
			Assert.True(File.Exists("test.png"));
		}
	}
}
