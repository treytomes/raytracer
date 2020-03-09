using raytracer.math;
using Xunit;

namespace raytracer.tests
{
	public class RayTests
	{
		public class SphereInteration
		{
			[Fact(DisplayName = "A ray intersects a sphere at two points.")]
			public void Can_intersect_sphere()
			{
				var r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
				var s = new Sphere();
				var xs = s.Intersect(r);
				Assert.Equal(2, xs.Count);
				Assert.Equal(new Intersection(4, s), xs[0]);
				Assert.Equal(new Intersection(6, s), xs[1]);
			}

			[Fact(DisplayName = "A ray intersects a sphere at a tangent.")]
			public void Can_tangent_a_sphere()
			{
				var r = new Ray(Tuple.Point(0, 1, -5), Tuple.Vector(0, 0, 1));
				var s = new Sphere();
				var xs = s.Intersect(r);
				Assert.Equal(2, xs.Count);
				Assert.Equal(new Intersection(5, s), xs[0]);
				Assert.Equal(new Intersection(5, s), xs[1]);
			}

			[Fact(DisplayName = "A ray misses a sphere.")]
			public void Can_miss_the_sphere()
			{
				var r = new Ray(Tuple.Point(0, 2, -5), Tuple.Vector(0, 0, 1));
				var s = new Sphere();
				var xs = s.Intersect(r);
				Assert.Null(xs);
			}

			[Fact(DisplayName = "A ray originates inside a sphere.")]
			public void Can_originate_inside_sphere()
			{
				var r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
				var s = new Sphere();
				var xs = s.Intersect(r);
				Assert.Equal(2, xs.Count);
				Assert.Equal(new Intersection(-1, s), xs[0]);
				Assert.Equal(new Intersection(1, s), xs[1]);
			}

			[Fact(DisplayName = "A sphere is behind a ray.")]
			public void Can_originate_after_sphere()
			{
				var r = new Ray(Tuple.Point(0, 0, 5), Tuple.Vector(0, 0, 1));
				var s = new Sphere();
				var xs = s.Intersect(r);
				Assert.Equal(2, xs.Count);
				Assert.Equal(new Intersection(-6, s), xs[0]);
				Assert.Equal(new Intersection(-4, s), xs[1]);
			}

			[Fact(DisplayName = "An intersection encapsulates t and object.")]
			public void Can_construct_intersection()
			{
				var s = new Sphere();
				var i = new Intersection(3.5f, s);
				Assert.Equal(3.5, i.T);
				Assert.Equal(s, i.Object);
			}

			[Fact(DisplayName = "Aggregating intersections.")]
			public void Can_aggregate_intersections()
			{
				var s = new Sphere();
				var i1 = new Intersection(1, s);
				var i2 = new Intersection(2, s);
				var xs = new IntersectionList(i1, i2);
				Assert.Equal(2, xs.Count);
				Assert.Equal(1, xs[0].T);
				Assert.Equal(2, xs[1].T);
			}

			[Fact(DisplayName = "Intersect sets the object on the intersection.")]
			public void Can_query_intersection()
			{
				var r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
				var s = new Sphere();
				var xs = s.Intersect(r);
				Assert.Equal(2, xs.Count);
				Assert.Equal(s, xs[0].Object);
				Assert.Equal(s, xs[1].Object);
			}

			[Fact(DisplayName = "The hit, when all intersections have positive t.")]
			public void Can_track_positive_hits()
			{
				var s = new Sphere();
				var i1 = new Intersection(1, s);
				var i2 = new Intersection(2, s);
				var xs = new IntersectionList(i1, i2);
				var i = xs.Hit();
				Assert.Equal(i1, i);
			}

			[Fact(DisplayName = "The hit, when some intersections have negative t.")]
			public void Can_track_some_negative_hits()
			{
				var s = new Sphere();
				var i1 = new Intersection(-1, s);
				var i2 = new Intersection(1, s);
				var xs = new IntersectionList(i1, i2);
				var i = xs.Hit();
				Assert.Equal(i2, i);
			}

			[Fact(DisplayName = "The hit, when all intersections have negative t.")]
			public void Can_track_all_negative_hits()
			{
				var s = new Sphere();
				var i1 = new Intersection(-2, s);
				var i2 = new Intersection(-1, s);
				var xs = new IntersectionList(i1, i2);
				var i = xs.Hit();
				Assert.Equal(Intersection.Empty(), i);
			}

			[Fact(DisplayName = "The hit is always the lowest nonnegative intersection.")]
			public void Can_acquire_nearest_hit()
			{
				var s = new Sphere();
				var i1 = new Intersection(5, s);
				var i2 = new Intersection(7, s);
				var i3 = new Intersection(-3, s);
				var i4 = new Intersection(2, s);
				var xs = new IntersectionList(i1, i2, i3, i4);
				var i = xs.Hit();
				Assert.Equal(i4, i);
			}

			[Fact(DisplayName = "A sphere's default transformation.")]
			public void Is_default_transform_identity()
			{
				var s = new Sphere();
				Assert.Equal(Matrix.Identity(4), s.Transform);
			}

			[Fact(DisplayName = "Changing a sphere's transformation.")]
			public void Can_transform_a_sphere()
			{
				var s = new Sphere();
				var t = Matrix.Translation(2, 3, 4);
				s.Transform = t;
				Assert.Equal(t, s.Transform);
			}

			[Fact(DisplayName = "Intersecting a scaled sphere with a ray.")]
			public void Can_intersect_scaled_sphere()
			{
				var r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
				var s = new Sphere();

				s.Transform = Matrix.Scaling(2, 2, 2);
				var xs = s.Intersect(r);

				Assert.Equal(2, xs.Count);
				Assert.Equal(3, xs[0].T);
				Assert.Equal(7, xs[1].T);
			}

			[Fact(DisplayName = "Intersecting a translated sphere with a ray.")]
			public void Can_intersect_translated_sphere()
			{
				var r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
				var s = new Sphere();

				s.Transform = Matrix.Translation(5, 0, 0);
				var xs = s.Intersect(r);

				Assert.Null(xs);
			}
		}

		[Fact(DisplayName = "Creating and querying a ray.")]
		public void Can_create_a_ray()
		{
			var origin = Tuple.Point(1, 2, 3);
			var direction = Tuple.Vector(4, 5, 6);
			var r = new Ray(origin, direction);
			Assert.Equal(origin, r.Origin);
			Assert.Equal(direction, r.Direction);
		}

		[Fact(DisplayName = "Computing a point from a distance.")]
		public void Can_calculate_distance()
		{
			var r = new Ray(Tuple.Point(2, 3, 4), Tuple.Vector(1, 0, 0));
			Assert.Equal(Tuple.Point(2, 3, 4), r.Position(0));
			Assert.Equal(Tuple.Point(3, 3, 4), r.Position(1));
			Assert.Equal(Tuple.Point(1, 3, 4), r.Position(-1));
			Assert.Equal(Tuple.Point(4.5f, 3, 4), r.Position(2.5f));
		}

		[Fact(DisplayName = "Translating a ray.")]
		public void Can_translate_a_ray()
		{
			var r = new Ray(Tuple.Point(1, 2, 3), Tuple.Vector(0, 1, 0));
			var m = Matrix.Translation(3, 4, 5);

			var r2 = r.Transform(m);
			Assert.Equal(Tuple.Point(4, 6, 8), r2.Origin);
			Assert.Equal(Tuple.Vector(0, 1, 0), r2.Direction);
		}

		[Fact(DisplayName = "Scaling a ray.")]
		public void Can_scale_a_ray()
		{
			var r = new Ray(Tuple.Point(1, 2, 3), Tuple.Vector(0, 1, 0));
			var m = Matrix.Scaling(2, 3, 4);

			var r2 = r.Transform(m);
			Assert.Equal(Tuple.Point(2, 6, 12), r2.Origin);
			Assert.Equal(Tuple.Vector(0, 3, 0), r2.Direction);
		}
	}
}
