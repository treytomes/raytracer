using System;
using System.Diagnostics.CodeAnalysis;

namespace raytracer.math
{
	/// <remarks>
	/// While it only makes sense to render colors with components in [0, 1], don't constrain the components to that range.
	/// The math being done on these colors will require that the values go out of range from time to time.
	/// </remarks>
	public struct Color : IEquatable<Color>
	{
		#region Fields

		public readonly float Red;
		public readonly float Green;
		public readonly float Blue;

		#endregion

		#region Constructors

		public Color(float red, float green, float blue)
		{
			Red = red;
			Green = green;
			Blue = blue;
		}

		#endregion

		#region Methods

		public override string ToString()
		{
			return $"Color({Red}, {Green}, {Blue})";
		}

		public bool Equals(Color other)
		{
			return MathHelper.Equals(Red, other.Red) && MathHelper.Equals(Green, other.Green) && MathHelper.Equals(Blue, other.Blue);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Color))
			{
				return false;
			}
			return Equals((Color)obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion

		#region Operators

		public static bool operator ==(Color left, Color right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Color left, Color right)
		{
			return !left.Equals(right);
		}

		public static Color operator +(Color left, Color right)
		{
			return new Color(left.Red + right.Red, left.Green + right.Green, left.Blue + right.Blue);
		}

		public static Color operator -(Color left, Color right)
		{
			return new Color(left.Red - right.Red, left.Green - right.Green, left.Blue - right.Blue);
		}

		public static Color operator *(Color t, float scale)
		{
			return new Color(t.Red * scale, t.Green * scale, t.Blue * scale);
		}

		public static Color operator *(float scale, Color c)
		{
			return new Color(c.Red * scale, c.Green * scale, c.Blue * scale);
		}

		public static Color operator /(Color c, float scale)
		{
			return new Color(c.Red / scale, c.Green / scale, c.Blue / scale);
		}

		/// <summary>
		/// Calculate the Hadamard / Schur product of 2 colors.
		/// </summary>
		public static Color operator *(Color left, Color right)
		{
			return new Color(left.Red * right.Red, left.Green * right.Green, left.Blue * right.Blue);
		}

		#endregion
	}
}
