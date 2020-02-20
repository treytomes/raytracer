using raytracer.math;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace raytracer.tests
{
	public class ColorTests
	{
		[Fact(DisplayName = "Colors are tuples.")]
		public void Can_construct_a_color()
		{
			var c = new Color(-0.5f, 0.4f, 1.7f);
			Assert.Equal(-0.5f, c.Red);
			Assert.Equal(0.4f, c.Green);
			Assert.Equal(1.7f, c.Blue);
		}

		[Fact(DisplayName = "Adding colors.")]
		public void Can_add_colors()
		{
			var c1 = new Color(0.9f, 0.6f, 0.75f);
			var c2 = new Color(0.7f, 0.1f, 0.25f);
			Assert.Equal(new Color(1.6f, 0.7f, 1.0f), c1 + c2);
		}

		[Fact(DisplayName = "Subtracting colors.")]
		public void Can_subtract_colors()
		{
			var c1 = new Color(0.9f, 0.6f, 0.75f);
			var c2 = new Color(0.7f, 0.1f, 0.25f);
			Assert.Equal(new Color(0.2f, 0.5f, 0.5f), c1 - c2);
		}

		[Fact(DisplayName = "Multiplying a color by a scalar.")]
		public void Can_scale_colors()
		{
			var c = new Color(0.2f, 0.3f, 0.4f);
			Assert.Equal(new Color(0.4f, 0.6f, 0.8f), c * 2);
		}

		[Fact(DisplayName = "Multiplying colors.")]
		public void Can_multiply_colors()
		{
			var c1 = new Color(1.0f, 0.2f, 0.4f);
			var c2 = new Color(0.9f, 1.0f, 0.1f);
			Assert.Equal(new Color(0.9f, 0.2f, 0.04f), c1 * c2);
		}
	}
}
