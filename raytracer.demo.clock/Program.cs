using raytracer.math;
using Console = System.Console;

namespace raytracer.demo.clock
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var canvas = ScreenController.Initialize(256, 256);

			var translate_scale = Matrix.Translation(128, 128, 0) * Matrix.Scaling(100, 100, 1);
			for (var hour = 1; hour <= 12; hour++)
			{
				var rotation = Matrix.RotationZ(hour * 2 * MathHelper.PI / 12.0f);
				var p = translate_scale * rotation * Tuple.Point(0, -1, 0);
				canvas.SetPixel((int)p.X, (int)p.Y, new Color(1, 1, 1));
			}
			
			canvas.Refresh();
			canvas.Save("clock.png");
		}
	}
}
