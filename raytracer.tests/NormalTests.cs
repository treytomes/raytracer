using raytracer.math;
using Xunit;

namespace raytracer.tests
{
	public class NormalTests
	{
		[Fact(DisplayName = "The normal on a sphere at a point on the x-axis.")]
		public void Can_get_x_normal()
		{
			var s = new Sphere();
			var n = s.NormalAt(Tuple.Point(1, 0, 0));
			Assert.Equal(Tuple.Vector(1, 0, 0), n);
		}

		[Fact(DisplayName = "The normal on a sphere at a point on the y-axis.")]
		public void Can_get_y_normal()
		{
			var s = new Sphere();
			var n = s.NormalAt(Tuple.Point(0, 1, 0));
			Assert.Equal(Tuple.Vector(0, 1, 0), n);
		}

		[Fact(DisplayName = "The normal on a sphere at a point on the z-axis.")]
		public void Can_get_z_normal()
		{
			var s = new Sphere();
			var n = s.NormalAt(Tuple.Point(0, 0, 1));
			Assert.Equal(Tuple.Vector(0, 0, 1), n);
		}

		[Fact(DisplayName = "The normal on a sphere at a point at a non-axial point.")]
		public void Can_get_nonaxial_normal()
		{
			var s = new Sphere();
			var n = s.NormalAt(Tuple.Point((float)System.Math.Sqrt(3) / 3, (float)System.Math.Sqrt(3) / 3, (float)System.Math.Sqrt(3) / 3));
			Assert.Equal(Tuple.Vector((float)System.Math.Sqrt(3) / 3, (float)System.Math.Sqrt(3) / 3, (float)System.Math.Sqrt(3) / 3), n);
		}

		[Fact(DisplayName = "The normal is a normalized vector.")]
		public void Is_normal_normalized()
		{
			var s = new Sphere();
			var n = s.NormalAt(Tuple.Point((float)System.Math.Sqrt(3) / 3, (float)System.Math.Sqrt(3) / 3, (float)System.Math.Sqrt(3) / 3));
			Assert.Equal(n.Normalize(), n);
		}

		[Fact(DisplayName = "Computing the normal on a translated sphere.")]
		public void Can_get_translated_normal()
		{
			var s = new Sphere();
			s.Transform = Matrix.Translation(0, 1, 0);

			var n = s.NormalAt(Tuple.Point(0, 1.70711f, -0.70711f));

			Assert.Equal(Tuple.Vector(0, 0.70711f, -0.70711f), n);
		}

		[Fact(DisplayName = "Computing the normal on a transformed sphere.")]
		public void Can_get_transformed_normal()
		{
			var s = new Sphere();
			var m = Matrix.Scaling(1, 0.5f, 1) * Matrix.RotationZ(MathHelper.PI / 5);
			s.Transform = m;

			var n = s.NormalAt(Tuple.Point(0, (float)System.Math.Sqrt(2) / 2, (float)-System.Math.Sqrt(2) / 2));

			Assert.Equal(Tuple.Vector(0, 0.97014f, -0.24254f), n);
		}
	}
}
