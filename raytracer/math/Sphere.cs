namespace raytracer.math
{
	public class Sphere
	{
		#region Fields

		private Matrix _transform;
		private Matrix _inverseTransform;

		#endregion

		#region Constructors

		public Sphere()
		{
			Transform = Matrix.Identity(4);
		}

		#endregion

		#region Properties

		public Matrix Transform
		{
			get
			{
				return _transform;
			}
			set
			{
				_transform = value;

				// It is *definitely* more efficient to store the inverse as a field on assigning the transform.
				_inverseTransform = _transform.Inverse();
			}
		}

		#endregion

		#region Methods

		public Intersection[] Intersect(Ray ray)
		{
			ray = ray.Transform(_inverseTransform);

			// The vector from the sphere's center, to the ray origin.
			// Remember: the sphere is centered at the world's origin.
			var sphere_to_ray = ray.Origin - Tuple.Point(0, 0, 0);

			var a = Tuple.Dot(ray.Direction, ray.Direction);
			var b = 2 * Tuple.Dot(ray.Direction, sphere_to_ray);
			var c = Tuple.Dot(sphere_to_ray, sphere_to_ray) - 1;

			var discriminant = b * b - 4 * a * c;

			if (discriminant < 0)
			{
				return null;
			}

			var t1 = (float)(-b - System.Math.Sqrt(discriminant)) / (2 * a);
			var t2 = (float)(-b + System.Math.Sqrt(discriminant)) / (2 * a);

			return new[] {
				new Intersection(t1, this),
				new Intersection(t2, this)
			};
		}

		public override string ToString()
		{
			return "Sphere()";
		}

		#endregion
	}
}
