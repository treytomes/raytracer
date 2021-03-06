﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
		private readonly float?[,] _cofactors;

		#endregion

		#region Constructors

		public Matrix(float[,] values)
		{
			Rows = values.GetLength(0);
			Columns = values.GetLength(1);
			_size = Rows * Columns;

			_values = new float[Rows, Columns];
			Array.Copy(values, 0, _values, 0, _size);

			_cofactors = new float?[Rows, Columns];
		}

		/// <remarks>
		/// The assumption is made here that <paramref name="values"/> describes a square matrix.
		/// </remarks>
		public Matrix(IEnumerable<float> values)
		{
			var sideLength = Math.Sqrt(values.Count());
			if ((int)sideLength != sideLength)
			{
				throw new Exception("Values must define a square matrix.");
			}
			Rows = (int)sideLength;
			Columns = (int)sideLength;
			_size = Rows * Columns;

			_values = new float[Rows, Columns];
			Array.Copy(values.ToArray(), 0, _values, 0, _size);

			_cofactors = new float?[Rows, Columns];
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
				if (_values[row, column] != value)
				{
					_values[row, column] = value;
					_cofactors[row, column] = null;
				}
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

		public static Matrix Translation(float x, float y, float z)
		{
			var m = Identity(4);
			m[0, 3] = x;
			m[1, 3] = y;
			m[2, 3] = z;
			return m;
		}

		public static Matrix Scaling(float x, float y, float z)
		{
			var m = Identity(4);
			m[0, 0] = x;
			m[1, 1] = y;
			m[2, 2] = z;
			return m;
		}

		public static Matrix RotationX(float radians)
		{
			var m = Matrix.Identity(4);
			m[1, 1] = (float)Math.Cos(radians);
			m[1, 2] = (float)-Math.Sin(radians);
			m[2, 1] = (float)Math.Sin(radians);
			m[2, 2] = (float)Math.Cos(radians);
			return m;
		}

		public static Matrix RotationY(float radians)
		{
			var m = Matrix.Identity(4);
			m[0, 0] = (float)Math.Cos(radians);
			m[0, 2] = (float)Math.Sin(radians);
			m[2, 0] = (float)-Math.Sin(radians);
			m[2, 2] = (float)Math.Cos(radians);
			return m;
		}

		public static Matrix RotationZ(float radians)
		{
			var m = Matrix.Identity(4);
			m[0, 0] = (float)Math.Cos(radians);
			m[0, 1] = (float)-Math.Sin(radians);
			m[1, 0] = (float)Math.Sin(radians);
			m[1, 1] = (float)Math.Cos(radians);
			return m;
		}

		public static Matrix Shearing(float xy, float xz, float yx, float yz, float zx, float zy)
		{
			var m = Matrix.Identity(4);
			m[0, 1] = xy;
			m[0, 2] = xz;
			m[1, 0] = yx;
			m[1, 2] = yz;
			m[2, 0] = zx;
			m[2, 1] = zy;
			return m;
		}

		public Matrix Transpose()
		{
			var m = Zero(Columns, Rows);
			for (var r = 0; r < Rows; r++)
			{
				for (var c = 0; c < Columns; c++)
				{
					m[c, r] = _values[r, c];
				}
			}
			return m;
		}

		public float Determinant()
		{
			if (Rows == 2)
			{
				return (_values[0, 0] * _values[1, 1]) - (_values[0, 1] * _values[1, 0]);
			}
			else
			{
				var det = 0f;
				for (var column = 0; column < Columns; column++)
				{
					det += _values[0, column] * Cofactor(0, column);
				}
				return det;
			}
		}

		public bool IsInvertible()
		{
			return Determinant() != 0;
		}

		public float Minor(int row, int column)
		{
			return Submatrix(row, column).Determinant();
		}

		public float Cofactor(int row, int column)
		{
			//return Minor(row, column) * ((((row + column) % 2) == 0) ? 1 : -1);
			var value = _cofactors[row, column];
			if (!value.HasValue)
			{
				value = Minor(row, column) * ((((row + column) % 2) == 0) ? 1 : -1);
				//value = Minor(row, column) * (((row + column + 1) % 2) * 2 - 1);
				_cofactors[row, column] = value;
			}
			return value.Value;
		}

		/// <summary>
		/// Return a sub-matrix with the given row and column removed.
		/// </summary>
		public Matrix Submatrix(int removeRow, int removeColumn)
		{
			var values = new float[Rows - 1, Columns - 1];
			var rd = 0;
			for (var r = 0; r < Rows; r++)
			{
				if (r == removeRow)
				{
					continue;
				}
				var cd = 0;
				for (var c = 0; c < Columns; c++)
				{
					if (c == removeColumn)
					{
						continue;
					}
					values[rd, cd] = _values[r, c];
					cd++;
				}
				rd++;
			}
			return new Matrix(values);
		}

		public Matrix Inverse()
		{
			var det = Determinant();
			var m = Zero(Rows, Columns);
			for (var row = 0; row < Rows; row++)
			{
				for (var column = 0; column < Columns; column++)
				{
					var cofactor = Cofactor(row, column);

					// Reversing (row, column) here does the transpose for us:
					m[column, row] = cofactor / det;
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
					if (!MathHelper.Equals(_values[r, c], other[r, c]))
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

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append($"Matrix({Rows},{Columns})=>{{");
			for (var r = 0; r < Rows; r++)
			{
				sb.Append("{");
				for (var c = 0; c < Columns; c++)
				{
					sb.Append(_values[r, c]);
					if (c != Columns - 1)
					{
						sb.Append(",");
					}
				}
				sb.Append("}");
				if (r != Rows - 1)
				{
					sb.Append(",");
				}
			}
			sb.Append("}");
			return sb.ToString();
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
