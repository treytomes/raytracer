using raytracer.math;

namespace raytracer.demo.sphere
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var canvas = ScreenController.Initialize(256, 256);

			var s = new Sphere();

			var ray_origin = Tuple.Point(0, 0, -5);
			var wall_z = 10f;
			var wall_size = 7f;
			var pixel_width = wall_size / canvas.Width;
			var pixel_height = wall_size / canvas.Height;
			var half = wall_size / 2;

			for (var y = 0; y < canvas.Height; y++)
			{
				// Compute the world y-coordinate.
				// Y is intentionally inverted from what you may expect (top = +half, bottom = -half).
				var world_y = half - pixel_height * y;

				for (var x = 0; x < canvas.Width; x++)
				{
					// Compute the world x-coordinate (left = -half, right = +half).
					var world_x = pixel_width * x - half;

					var position = Tuple.Point(world_x, world_y, wall_z);

					var r = new Ray(ray_origin, (position - ray_origin).Normalize());
					var xs = s.Intersect(r);
					if (xs != null)
					{
						var hit = new IntersectionList(xs).Hit();
						if (hit.Object != null)
						{
							canvas.SetPixel(x, y, new Color(1, 0, 0));
						}
					}
				}
			}

			canvas.Refresh();
			canvas.Save("sphere.png");
		}
	}
}
