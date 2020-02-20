using System;
using Tuple = raytracer.math.Tuple;

namespace raytracer.demo.projectiles
{
	/// <summary>
	/// The projectile demo, as described at the end of chapter 1 of the Ray Tracer Challenge.
	/// </summary>
	public static class Program
	{
		public static void Main(string[] args)
		{

			// Projectile starts 1 unit above the origin.
			// Velocity is normalized to 1 unit/tick.
			var p = (position: Tuple.Point(0, 1, 0), velocity: Tuple.Vector(1, 1, 0).Normalize());

			// Gravity -0.1 units/tick, and wind is -0.01 unit/tick.
			var e = (gravity: Tuple.Vector(0, -0.1f, 0), wind: Tuple.Vector(-0.01f, 0, 0));

			var numTicks = 0;
			Console.WriteLine($"[{numTicks}]: {p.position}");
			while (p.position.Y >= 0)
			{
				p = Tick(e, p);
				numTicks++;
				Console.WriteLine($"[{numTicks}]: {p.position}");
			}
		}

		private static (Tuple position, Tuple velocity) Tick((Tuple gravity, Tuple wind) env, (Tuple position, Tuple velocity) proj)
		{
			var position = proj.position + proj.velocity;
			var velocity = proj.velocity + env.gravity + env.wind;
			return (position, velocity);
		}
	}
}
