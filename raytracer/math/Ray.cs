using System;
using System.Collections.Generic;
using System.Text;

namespace raytracer.math
{
	public struct Ray
	{
		#region Fields

		public readonly Tuple Origin;
		public readonly Tuple Direction;

		#endregion

		#region Constructors

		public Ray(Tuple origin, Tuple direction)
		{
			Origin = origin;
			Direction = direction;
		}

		#endregion

		#region Methods

		public Ray Transform(Matrix m)
		{
			return new Ray(m * Origin, m * Direction);
		}

		/// <summary>
		/// Find the position of this ray at time <paramref name="t"/>.
		/// </summary>
		public Tuple Position(float t)
		{
			return Origin + Direction * t;
		}

		#endregion
	}
}
