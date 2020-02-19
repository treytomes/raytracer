using System;

namespace raytracer.math
{
	/// <remarks>
	/// If you add 2 points, W=2, which is a non-sensical result.  But then, why would anyone want to add points?
	/// A lot of the magic here depends on W being either 0 or 1, and nothing else.
	/// </remarks>
	public struct Tuple : IEquatable<Tuple>
	{
		#region Fields

		public readonly float X;

		public readonly float Y;

		public readonly float Z;

		public readonly float W;

		#endregion

		#region Constructors

		public Tuple(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		#endregion

		#region Methods

		public static Tuple Point(float x, float y, float z)
		{
			return new Tuple(x, y, z, 1.0f);
		}

		public static Tuple Vector(float x, float y, float z)
		{
			return new Tuple(x, y, z, 0.0f);
		}

		/// <summary>
		/// Dot product.  The smaller the result, the larger the angle between vectors.
		/// This is also the cosine of the angle between vectors.
		/// </summary>
		/// <remarks>
		/// TODO: https://betterexplained.com/articles/vector-calculus-understanding-the-dot-product/
		/// </remarks>
		public static float Dot(Tuple left, Tuple right)
		{
			return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
		}

		/// <summary>
		/// The cross product between two vectors (and they *must* be vectors, not points),
		/// yields a new vector that is perpendicular to both of the inputs.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Tuple Cross(Tuple left, Tuple right)
		{
			if (!left.IsVector())
			{
				throw new ArgumentException("The tuple must be a vector.", nameof(left));
			}
			if (!right.IsVector())
			{
				throw new ArgumentException("The tuple must be a vector.", nameof(right));
			}

			return Vector((left.Y * right.Z) - (left.Z * right.Y), (left.Z * right.X) - (left.X * right.Z), (left.X * right.Y) - (left.Y * right.X));
		}

		public bool IsPoint()
		{
			return W == 1.0;
		}

		public bool IsVector()
		{
			return W == 0.0f;
		}

		public float Magnitude()
		{
			return (float)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
		}

		public Tuple Normalize()
		{
			var magnitude = Magnitude();
			return new Tuple(X / magnitude, Y / magnitude, Z / magnitude, W / magnitude);
		}

		public bool Equals(Tuple other)
		{
			return MathHelper.Equals(X, other.X) && MathHelper.Equals(Y, other.Y) && MathHelper.Equals(Z, other.Z) && MathHelper.Equals(W, other.W);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Tuple))
			{
				return false;
			}
			return Equals((Tuple)obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion

		#region Operators

		public static bool operator ==(Tuple left, Tuple right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Tuple left, Tuple right)
		{
			return !left.Equals(right);
		}

		public static Tuple operator +(Tuple left, Tuple right)
		{
			return new Tuple(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
		}

		public static Tuple operator -(Tuple left, Tuple right)
		{
			return new Tuple(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
		}

		public static Tuple operator -(Tuple t)
		{
			return new Tuple(-t.X, -t.Y, -t.Z, -t.W);
		}

		public static Tuple operator *(Tuple t, float scale)
		{
			return new Tuple(t.X * scale, t.Y * scale, t.Z * scale, t.W * scale);
		}

		public static Tuple operator *(float scale, Tuple t)
		{
			return new Tuple(t.X * scale, t.Y * scale, t.Z * scale, t.W * scale);
		}

		public static Tuple operator /(Tuple t, float scale)
		{
			return new Tuple(t.X / scale, t.Y / scale, t.Z / scale, t.W / scale);
		}

		#endregion
	}
}
