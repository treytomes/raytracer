using raytracer.math;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace raytracer
{
	public class RayTracerGameState : GameState
	{
		private System.Random _random = new System.Random();
		private int _frames = 0;

		private Sphere _sphere;
		private Color _color = new Color(1, 0, 0);
		private Tuple _rayOrigin = Tuple.Point(0, 0, -5);
		private float _wallZ = 10f;
		private float _wallWidth = 7f;
		private float _wallHeight = 7f;
		private float _pixelWidth;
		private float _pixelHeight;
		private float _half;


		public RayTracerGameState()
			: base()
		{
			_sphere = new Sphere();
			//_sphere.Transform = Matrix.Scaling(1, 0.5f, 1);

			_pixelWidth = _wallWidth / ScreenWidth;
			_pixelHeight = _wallHeight / ScreenHeight;
			_half = _wallWidth / 2;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			//var delta = gameTime.TotalTime.TotalSeconds * 16f;
			var delta = gameTime.TotalTime.TotalMilliseconds / 32;
			_sphere.Transform =
				Matrix.Scaling(1, (float)System.Math.Sin(MathHelper.DegreesToRadians((float)delta)), 1) *
				Matrix.Scaling((float)System.Math.Cos(MathHelper.DegreesToRadians((float)delta)), 1, 1) *
				Matrix.Shearing((float)System.Math.Sin(MathHelper.DegreesToRadians((float)delta)), 0, 0, 0, 0, 0);
		}

		public override void Render(GameTime gameTime)
		{
			base.Render(gameTime);

			Parallel.For(0, ScreenHeight, new ParallelOptions() { MaxDegreeOfParallelism = ScreenHeight }, y =>
			{
				// Compute the world y-coordinate.
				// Y is intentionally inverted from what you may expect (top = +half, bottom = -half).
				var worldY = _half - _pixelHeight * y;

				Parallel.For(0, ScreenWidth, new ParallelOptions() { MaxDegreeOfParallelism = ScreenWidth }, x =>
				{
					// Compute the world x-coordinate (left = -half, right = +half).
					var worldX = _pixelWidth * x - _half;

					var position = Tuple.Point(worldX, worldY, _wallZ);

					var r = new Ray(_rayOrigin, (position - _rayOrigin).Normalize());
					var xs = _sphere.Intersect(r);

					ScreenController.Instance.SetPixel(x, y, new Color(0, 0, 0));
					if (xs != null)
					{
						var hit = new IntersectionList(xs).Hit();
						if (hit.Object != null)
						{
							ScreenController.Instance.SetPixel(x, y, _color);
						}
					}
				});
			});

			//for (var y = 0; y < ScreenHeight; y++)
			//{
			//	// Compute the world y-coordinate.
			//	// Y is intentionally inverted from what you may expect (top = +half, bottom = -half).
			//	var worldY = _half - _pixelHeight * y;

			//	for (var x = 0; x < ScreenWidth; x++)
			//	{
			//		// Compute the world x-coordinate (left = -half, right = +half).
			//		var worldX = _pixelWidth * x - _half;

			//		var position = Tuple.Point(worldX, worldY, _wallZ);

			//		var r = new Ray(_rayOrigin, (position - _rayOrigin).Normalize());
			//		var xs = _sphere.Intersect(r);

			//		ScreenController.Instance.SetPixel(x, y, new Color(0, 0, 0));
			//		if (xs != null)
			//		{
			//			var hit = new IntersectionList(xs).Hit();
			//			if (hit.Object != null)
			//			{
			//				ScreenController.Instance.SetPixel(x, y, _color);
			//			}
			//		}
			//	}
			//}

			//_frames++;
			//if (gameTime.TotalTime.TotalSeconds != 0)
			//{
			//	Dispatcher.CurrentDispatcher.BeginInvoke(() => App.Current.MainWindow.Title = $"raytracer, FPS={_frames / gameTime.TotalTime.TotalSeconds}");
			//}
		}
	}
}
