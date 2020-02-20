using System;

namespace raytracer.math
{
	/// <summary>
	/// Representation for both Vectors and Points.
	/// </summary>
	/// <remarks>
	/// If you add 2 points, W=2, which is a non-sensical result.  But then, why would anyone want to add points?
	/// A lot of the magic here depends on W being either 0 or 1, and nothing else.
	/// 
	/// I spent a lot of time trying to decide whether to have separate classes for Point and Vector;
	/// whether to use classes or structures.  Structures should be faster in this case, but also
	/// remove the ability to create an inheritance chain.
	/// 
	/// Likewise, I am using float instead of double or decimal for the sake of speed.  The output quality should be "good enough".
	/// 
	/// I also hate the name collision with System.Tuple.  Maybe a better name for this class will occur to me later.
	/// 
	/// I may ultimately replace this with System.Windows.Media.Media3D.[Point3D, Vector3D].
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
		/// <remarks>
		/// I'm not implementing the * operator on this struct.  Would the * operator be doing
		/// a cross product or a dot product?  I'd rather make things clear.
		/// </remarks>
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

		public override string ToString()
		{
			if (IsPoint())
			{
				return $"Point({X}, {Y}, {Z})";
			}
			else if (IsVector())
			{
				return $"Vector({X}, {Y}, {Z})";
			}
			else
			{
				return $"Tuple({X}, {Y}, {Z}, {W})";
			}
		}

		public bool IsPoint()
		{
			return W == 1.0;
		}

		public bool IsVector()
		{
			return W == 0.0f;
		}

		/// <summary>
		/// Calculate the magnitude / length of a vector, including the W component.
		/// </summary>
		/// <remarks>
		/// This calculation will break down if you try to take the magnitude of a point.
		/// </remarks>
		/// <returns>The magnitude / length of the vector.</returns>
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
