using raytracer.math;
using System;
using Tuple = raytracer.math.Tuple;

namespace raytracer.demo.projectiles
{
	/// <summary>
	/// The projectile demo, as described at the end of chapter 1 of the Ray Tracer Challenge.
	/// </summary>
	public static class Program
	{
		public static void Main()
		{
			var a = new Matrix(new[,] {
				{ 3f, -9, 7, 3 },
				{ 3, -8, 2, -9 },
				{ -4, 4, 4, 1 },
				{ -6, 5, -1, 1 }
			});

			//Console.WriteLine(a);
			Console.WriteLine(Matrix.Identity(4));
			Console.WriteLine(Matrix.Identity(4).Inverse());
			Console.WriteLine(a.Inverse().Transpose());
			Console.WriteLine(a.Transpose().Inverse());

			var i = Matrix.Identity(4);

			Console.WriteLine(i * new Tuple(4, 5, 6, 7));

			i[0, 0] = 2;

			Console.WriteLine(i * new Tuple(4, 5, 6, 7));
		}

		public static void ProjectileLauncher()
		{
			var start = Tuple.Point(0, 1, 0);
			var velocity = Tuple.Vector(1, 1.8f, 0).Normalize() * 11.25f;

			// Projectile starts 1 unit above the origin.
			// Velocity is normalized to 1 unit/tick.
			var p = (position: start, velocity: velocity);

			// Gravity -0.1 units/tick, and wind is -0.01 unit/tick.
			var gravity = Tuple.Vector(0, -0.1f, 0);
			var wind = Tuple.Vector(-0.01f, 0, 0);
			var e = (gravity: gravity, wind: wind);

			var c = ScreenController.Initialize(900, 550);

			var numTicks = 0;
			Console.WriteLine($"[{numTicks}]: {p.position}");
			if ((p.position.X >= 0) && (p.position.X < c.Width) && (p.position.Y >= 0) && (p.position.Y < c.Height))
			{
				c.SetPixel((int)p.position.X, c.Height - (int)p.position.Y, new Color(1, 0.5f, 0.5f));
			}
			while (p.position.Y >= 0)
			{
				p = Tick(e, p);
				if ((p.position.X >= 0) && (p.position.X < c.Width) && (p.position.Y >= 0) && (p.position.Y < c.Height))
				{
					c.SetPixel((int)p.position.X, c.Height - (int)p.position.Y, new Color(1, 0.5f, 0.5f));
				}
				numTicks++;
				Console.WriteLine($"[{numTicks}]: {p.position}");
			}
			c.Refresh();
			c.Save("projectile.png");
		}

		private static (Tuple position, Tuple velocity) Tick((Tuple gravity, Tuple wind) env, (Tuple position, Tuple velocity) proj)
		{
			var position = proj.position + proj.velocity;
			var velocity = proj.velocity + env.gravity + env.wind;
			return (position, velocity);
		}
	}
}
