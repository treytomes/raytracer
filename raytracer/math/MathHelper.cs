using System;

namespace raytracer.math
{
	public static class MathHelper
	{
		/// <summary>
		/// The number of decimal places we care about.
		/// </summary>
		public const int PRECISION = 5;

		/// <summary>
		/// Numbers are considered close enough if they are this far apart.
		/// </summary>
		/// <remarks>
		/// 1 * 10 ^ -5, where 5 is the value of PRECISION.
		/// </remarks>
		public const float EPSILON = 0.00001f;

		/// <summary>
		/// Are <paramref name="a"/> and <paramref name="b"/> approximately equal?
		/// </summary>
		public static bool Equals(float a, float b)
		{
			return Math.Abs(a - b) < EPSILON;
		}
	}
}
