using raytracer.math;
using Xunit;

namespace raytracer.tests
{
	public class TransformationTests
	{
		public class Translation
		{
			[Fact(DisplayName = "Multiplying by a translation matrix.")]
			public void Can_translate_a_point()
			{
				var transform = Matrix.Translation(5, -3, 2);
				var p = Tuple.Point(-3, 4, 5);
				Assert.Equal(Tuple.Point(2, 1, 7), transform * p);
			}

			[Fact(DisplayName = "Multiplying by the inverse of a translation matrix.")]
			public void Can_translate_a_point_backwards()
			{
				var transform = Matrix.Translation(5, -3, 2);
				var inv = transform.Inverse();
				var p = Tuple.Point(-3, 4, 5);
				Assert.Equal(Tuple.Point(-8, 7, 3), inv * p);
			}

			[Fact(DisplayName = "Translation does not affect vectors.")]
			public void Cannot_translate_a_vector()
			{
				var transform = Matrix.Translation(5, -3, 2);
				var v = Tuple.Vector(-3, 4, 5);
				Assert.Equal(v, transform * v);
			}
		}

		public class Scaling
		{
			[Fact(DisplayName = "A scaling matrix applied to a point.")]
			public void Can_scale_a_point()
			{
				var transform = Matrix.Scaling(2, 3, 4);
				var p = Tuple.Point(-4, 6, 8);
				Assert.Equal(Tuple.Point(-8, 18, 32), transform * p);
			}

			[Fact(DisplayName = "A scaling matrix applied to a vector.")]
			public void Can_scale_a_vector()
			{
				var transform = Matrix.Scaling(2, 3, 4);
				var v = Tuple.Vector(-4, 6, 8);
				Assert.Equal(Tuple.Vector(-8, 18, 32), transform * v);
			}

			[Fact(DisplayName = "Multiplying by the inverse of a scaling matrix.")]
			public void Can_unscale_a_vector()
			{
				var transform = Matrix.Scaling(2, 3, 4);
				var inv = transform.Inverse();
				var v = Tuple.Vector(-4, 6, 8);
				Assert.Equal(Tuple.Vector(-2, 2, 2), inv * v);
			}

			[Fact(DisplayName = "Reflection is scaling by a negative value.")]
			public void Can_reflect_a_point()
			{
				var transform = Matrix.Scaling(-1, 1, 1);
				var p = Tuple.Point(2, 3, 4);
				Assert.Equal(Tuple.Point(-2, 3, 4), transform * p);
			}
		}

		public class Rotation
		{
			[Fact(DisplayName = "Rotating a point around the x-axis.")]
			public void Can_rotate_x()
			{
				var p = Tuple.Point(0, 1, 0);
				var half_quarter = Matrix.RotationX(MathHelper.PI / 4);
				var full_quarter = Matrix.RotationX(MathHelper.PI / 2);
				Assert.Equal(Tuple.Point(0, (float)System.Math.Sqrt(2) / 2, (float)System.Math.Sqrt(2) / 2), half_quarter * p);
				Assert.Equal(Tuple.Point(0, 0, 1), full_quarter * p);
			}

			[Fact(DisplayName = "The inverse of an x-rotation rotates in the opposite direction.")]
			public void Can_invert_rotation_x()
			{
				var p = Tuple.Point(0, 1, 0);
				var half_quarter = Matrix.RotationX(MathHelper.PI / 4);
				var inv = half_quarter.Inverse();
				Assert.Equal(Tuple.Point(0, (float)System.Math.Sqrt(2) / 2, (float)System.Math.Sqrt(2) / -2), inv * p);
			}

			[Fact(DisplayName = "Rotating a point around the y-axis.")]
			public void Can_rotate_y()
			{
				var p = Tuple.Point(0, 0, 1);
				var half_quarter = Matrix.RotationY(MathHelper.PI / 4);
				var full_quarter = Matrix.RotationY(MathHelper.PI / 2);
				Assert.Equal(Tuple.Point((float)System.Math.Sqrt(2) / 2, 0, (float)System.Math.Sqrt(2) / 2), half_quarter * p);
				Assert.Equal(Tuple.Point(1, 0, 0), full_quarter * p);
			}

			[Fact(DisplayName = "Rotating a point around the z-axis.")]
			public void Can_rotate_z()
			{
				var p = Tuple.Point(0, 1, 0);
				var half_quarter = Matrix.RotationZ(MathHelper.PI / 4);
				var full_quarter = Matrix.RotationZ(MathHelper.PI / 2);
				Assert.Equal(Tuple.Point((float)-System.Math.Sqrt(2) / 2, (float)System.Math.Sqrt(2) / 2, 0), half_quarter * p);
				Assert.Equal(Tuple.Point(-1, 0, 0), full_quarter * p);
			}
		}

		public class Shearing
		{
			[Theory(DisplayName = "A shearing transformation moves 1 coordinate in proportion to the other 2.")]
			[InlineData(1, 0, 0, 0, 0, 0, 2, 3, 4, 5, 3, 4)]
			[InlineData(0, 1, 0, 0, 0, 0, 2, 3, 4, 6, 3, 4)]
			[InlineData(0, 0, 1, 0, 0, 0, 2, 3, 4, 2, 5, 4)]
			[InlineData(0, 0, 0, 1, 0, 0, 2, 3, 4, 2, 7, 4)]
			[InlineData(0, 0, 0, 0, 1, 0, 2, 3, 4, 2, 3, 6)]
			[InlineData(0, 0, 0, 0, 0, 1, 2, 3, 4, 2, 3, 7)]
			public void Can_shear_a_point(float xy, float xz, float yx, float yz, float zx, float zy, float xi, float yi, float zi, float xo, float yo, float zo)
			{
				var transform = Matrix.Shearing(xy, xz, yx, yz, zx, zy);
				var p = Tuple.Point(xi, yi, zi);
				Assert.Equal(Tuple.Point(xo, yo, zo), transform * p);
			}
		}

		[Fact(DisplayName = "Individual transformations are applied in sequence.")]
		public void Can_sequence_tranformations()
		{
			var p = Tuple.Point(1, 0, 1);
			var A = Matrix.RotationX(MathHelper.PI / 2);
			var B = Matrix.Scaling(5, 5, 5);
			var C = Matrix.Translation(10, 5, 7);

			// Apply rotation first.
			var p2 = A * p;
			Assert.Equal(Tuple.Point(1, -1, 0), p2);

			// Then apply scaling.
			var p3 = B * p2;
			Assert.Equal(Tuple.Point(5, -5, 0), p3);

			// The apply translation.
			var p4 = C * p3;
			Assert.Equal(Tuple.Point(15, 0, 7), p4);
		}

		[Fact(DisplayName = "Chained transformations must be applied in reverse order.")]
		public void Can_combine_transformations()
		{
			var p = Tuple.Point(1, 0, 1);
			var A = Matrix.RotationX(MathHelper.PI / 2);
			var B = Matrix.Scaling(5, 5, 5);
			var C = Matrix.Translation(10, 5, 7);

			var T = C * B * A;
			Assert.Equal(Tuple.Point(15, 0, 7), T * p);
		}
	}
}
