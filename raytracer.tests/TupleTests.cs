using raytracer.math;
using System.Collections.Generic;
using Xunit;

namespace raytracer.tests
{
	public class TupleTests
	{
		[Fact(DisplayName = "A tuple with w=1.0 is a point.")]
		public void Can_construct_a_point()
		{
			var a = new Tuple(4.3f, -4.2f, 3.1f, 1.0f);
			Assert.Equal(4.3f, a.X);
			Assert.Equal(-4.2f, a.Y);
			Assert.Equal(3.1f, a.Z);
			Assert.Equal(1.0f, a.W);
			Assert.True(a.IsPoint());
			Assert.False(a.IsVector());
		}

		[Fact(DisplayName = "A tuple with w=0.0 is a vector.")]
		public void Can_construct_a_vector()
		{
			var a = new Tuple(4.3f, -4.2f, 3.1f, 0.0f);
			Assert.Equal(4.3f, a.X);
			Assert.Equal(-4.2f, a.Y);
			Assert.Equal(3.1f, a.Z);
			Assert.Equal(0.0f, a.W);
			Assert.False(a.IsPoint());
			Assert.True(a.IsVector());
		}

		[Fact(DisplayName = "Tuple.Point() creates tuples with W=1.")]
		public void Can_construct_points_from_factory()
		{
			var p = Tuple.Point(4, -4, 3);
			Assert.True(p.IsPoint());
			Assert.Equal(new Tuple(4, -4, 3, 1), p);
		}

		[Fact(DisplayName = "Tuple.Vector() creates tuples with W=0.")]
		public void Can_construct_vectors_from_factory()
		{
			var v = Tuple.Vector(4, -4, 3);
			Assert.True(v.IsVector());
			Assert.Equal(new Tuple(4, -4, 3, 0), v);
		}

		[Fact(DisplayName = "Adding two tuples.")]
		public void Can_add_tuples()
		{
			var a1 = new Tuple(3, -2, 5, 1);
			var a2 = new Tuple(-2, 3, 1, 0);
			Assert.Equal(new Tuple(1, 1, 6, 1), a1 + a2);
		}

		[Fact(DisplayName = "Subtracting two tuples.")]
		public void Can_subtract_tuples()
		{
			var p1 = Tuple.Point(3, 2, 1);
			var p2 = Tuple.Point(5, 6, 7);
			Assert.Equal(Tuple.Vector(-2, -4, -6), p1 - p2);
		}

		[Fact(DisplayName = "Subtracting a vector from a point.")]
		public void Can_subtract_vector_from_point()
		{
			var p = Tuple.Point(3, 2, 1);
			var v = Tuple.Vector(5, 6, 7);
			Assert.Equal(Tuple.Point(-2, -4, -6), p - v);
		}

		[Fact(DisplayName = "Subtracting two vectors.")]
		public void Can_subtract_vector_from_vector()
		{
			var v1 = Tuple.Vector(3, 2, 1);
			var v2 = Tuple.Vector(5, 6, 7);
			Assert.Equal(Tuple.Vector(-2, -4, -6), v1 - v2);
		}

		[Fact(DisplayName = "Subtracting a vector from the zero vector.")]
		public void Can_subtract_a_vector_from_zero()
		{
			var zero = Tuple.Vector(0, 0, 0);
			var v = Tuple.Vector(1, -2, 3);
			Assert.Equal(Tuple.Vector(-1, 2, -3), zero - v);
		}

		[Fact(DisplayName = "Negating a tuple.")]
		public void Can_negate_a_tuple()
		{
			var a = new Tuple(1, -2, 3, -4);
			Assert.Equal(new Tuple(-1, 2, -3, 4), -a);
		}

		[Fact(DisplayName = "Multiplying a tuple by a scalar.")]
		public void Can_scale_a_tuple()
		{
			var a = new Tuple(1, -2, 3, -4);
			Assert.Equal(new Tuple(3.5f, -7, 10.5f, -14), a * 3.5f);
		}

		[Fact(DisplayName = "Multiplying a tuple by a fraction.")]
		public void Can_fractionally_scale_a_tuple()
		{
			var a = new Tuple(1, -2, 3, -4);
			Assert.Equal(new Tuple(0.5f, -1, 1.5f, -2), 0.5f * a);
		}

		[Fact(DisplayName = "Dividing a tuple by a scalar.")]
		public void Can_divide_a_tuple()
		{
			var a = new Tuple(1, -2, 3, -4);
			Assert.Equal(new Tuple(0.5f, -1, 1.5f, -2), a / 2);
		}

		[Theory(DisplayName = "Computing the magnitude of a vector.")]
		//[InlineData(1, 0, 0, 1)]
		//[InlineData(0, 1, 0, 1)]
		//[InlineData(0, 0, 1, 1)]
		//[InlineData(1, 2, 3, 3.741657386773941f)]
		//[InlineData(-1, -2, -3, 3.741657386773941f)]
		[MemberData(nameof(VectorMagnitudes))]
		public void Can_compute_vector_magnitude(float x, float y, float z, float magnitude)
		{
			var v = Tuple.Vector(x, y, z);
			Assert.Equal(magnitude, v.Magnitude());
		}

		public static IEnumerable<object[]> VectorMagnitudes
		{
			get
			{
				yield return new object[] { 1f, 0, 0, 1 };
				yield return new object[] { 0f, 1, 0, 1 };
				yield return new object[] { 0f, 0, 1, 1 };
				yield return new object[] { 1, 2, 3, System.Math.Sqrt(14) };
				yield return new object[] { -1, -2, -3, System.Math.Sqrt(14) };
			}
		}

		[Theory(DisplayName = "Normalizing a vector.")]
		[InlineData(4, 0, 0, 1, 0, 0)]
		[InlineData(1, 2, 3, 0.26726f, 0.53452f, 0.80178f)]
		public void Can_normalize_a_vector(float vx, float vy, float vz, float nx, float ny, float nz)
		{
			var v = Tuple.Vector(vx, vy, vz);
			var n = Tuple.Vector(nx, ny, nz);
			Assert.Equal(n, v.Normalize());
		}

		[Fact(DisplayName = "The magnitude of a normalized vector.")]
		public void Normalized_vector_magnitude_is_1()
		{
			var v = Tuple.Vector(1, 2, 3);
			Assert.Equal(1, v.Normalize().Magnitude(), MathHelper.PRECISION);
		}

		[Fact(DisplayName = "The dot product of two tuples.")]
		public void Can_dot_product_2_tuples()
		{
			var a = Tuple.Vector(1, 2, 3);
			var b = Tuple.Vector(2, 3, 4);
			Assert.Equal(20, Tuple.Dot(a, b));
		}

		[Fact(DisplayName = "The cross product of two vectors.")]
		public void Can_cross_2_vectors()
		{
			var a = Tuple.Vector(1, 2, 3);
			var b = Tuple.Vector(2, 3, 4);
			Assert.Equal(Tuple.Vector(-1, 2, -1), Tuple.Cross(a, b));
			Assert.Equal(Tuple.Vector(1, -2, 1), Tuple.Cross(b, a));
		}
	}
}
