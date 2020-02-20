using System;
using System.Collections.Generic;
using System.Text;

namespace raytracer
{
	public class RayTracerGameState : GameState
	{
		private Random _random = new Random();

		public RayTracerGameState()
			: base()
		{
		}

		public override void Render(GameTime gameTime)
		{
			base.Render(gameTime);

			var color = new math.Color((float)_random.NextDouble(), (float)_random.NextDouble(), (float)_random.NextDouble());
			for (var y = 0; y < ScreenHeight; y++)
			{
				for (var x = 0; x < ScreenWidth; x++)
				{
					ScreenController.Instance.SetPixel(x, y, color);
				}
			}
		}
	}
}
