using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace raytracer.math
{
	/// <summary>
	/// Describe a 4x4 matrix.
	/// </summary>
	public struct Matrix : IEquatable<Matrix>
	{
		#region Fields

		public readonly int Rows;
		public readonly int Columns;
		private readonly int _size;
		private readonly float[,] _values;

		#endregion

		#region Constructors

		public Matrix(float[,] values)
		{
			Rows = values.GetLength(0);
			Columns = values.GetLength(1);
			_size = Rows * Columns;

			_values = new float[Rows, Columns];
			Array.Copy(values, 0, _values, 0, _size);
		}

		#endregion

		#region Properties

		public float this[int row, int column]
		{
			get
			{
				return _values[row, column];
			}
			set
			{
				_values[row, column] = value;
			}
		}

		#endregion

		#region Methods

		public static Matrix Zero(int rows, int columns)
		{
			return new Matrix(new float[rows, columns]);
		}

		public static Matrix Identity(int size = 4)
		{
			var m = new Matrix(new float[size, size]);
			for (var n = 0; n < size; n++)
			{
				m[n, n] = 1;
			}
			return m;
		}

		public Matrix Transpose()
		{
			var m = Zero(Columns, Rows);
			for (var r = 0; r < Rows; r++)
			{
				for (var c = 0; c < Columns; c++)
				{
					m[c, r] = this[r, c];
				}
			}
			return m;
		}

		public bool Equals(Matrix other)
		{
			for (var r = 0; r < Rows; r++)
			{
				for (var c = 0; c < Columns; c++)
				{
					if (!MathHelper.Equals(this[r, c], other[r, c]))
					{
						return false;
					}
				}
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Matrix))
			{
				return false;
			}
			return Equals((Matrix)obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion

		#region Operators

		public static bool operator ==(Matrix left, Matrix right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Matrix left, Matrix right)
		{
			return !left.Equals(right);
		}

		/// <remarks>
		/// This will fail if left.Columns != right.Rows.
		/// </remarks>
		public static Matrix operator *(Matrix left, Matrix right)
		{
			var m = Zero(left.Rows, right.Columns);
			for (var r = 0; r < left.Rows; r++)
			{
				for (var c = 0; c < right.Columns; c++)
				{
					for (var n = 0; n < right.Rows; n++)
					{
						m[r, c] += left[r, n] * right[n, c];
					}
				}
			}
			return m;
		}

		/// <remarks>
		/// This will fail for matrices != 4x4.
		/// </remarks>
		public static Tuple operator *(Matrix left, Tuple right)
		{
			return new Tuple(
				left[0, 0] * right.X + left[0, 1] * right.Y + left[0, 2] * right.Z + left[0, 3] * right.W,
				left[1, 0] * right.X + left[1, 1] * right.Y + left[1, 2] * right.Z + left[1, 3] * right.W,
				left[2, 0] * right.X + left[2, 1] * right.Y + left[2, 2] * right.Z + left[2, 3] * right.W,
				left[3, 0] * right.X + left[3, 1] * right.Y + left[3, 2] * right.Z + left[3, 3] * right.W
			);
		}

		#endregion
	}
}
