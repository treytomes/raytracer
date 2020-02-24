using raytracer.math;
using Xunit;

namespace raytracer.tests
{
	public class MatrixTests
	{
		[Fact(DisplayName = "Constructing and inspecting a 4x4 matrix.")]
		public void Can_construct_a_matrix()
		{
			var m = new Matrix(new[,] {
				{ 1, 2, 3, 4 },
				{ 5.5f, 6.5f, 7.5f, 8.5f },
				{ 9, 10, 11, 12 },
				{ 13.5f, 14.5f, 15.5f, 16.5f }
			});

			Assert.Equal(1, m[0, 0]);
			Assert.Equal(4, m[0, 3]);
			Assert.Equal(5.5, m[1, 0]);
			Assert.Equal(7.5, m[1, 2]);
			Assert.Equal(11, m[2, 2]);
			Assert.Equal(13.5, m[3, 0]);
			Assert.Equal(15.5, m[3, 2]);
		}

		[Fact(DisplayName = "A 2x2 matrix ought to be representable.")]
		public void Can_construct_2x2_matrix()
		{
			var m = new Matrix(new[,] {
				{ -3f, 5 },
				{ 1, -2 },
			});
			Assert.Equal(-3, m[0, 0]);
			Assert.Equal(5, m[0, 1]);
			Assert.Equal(1, m[1, 0]);
			Assert.Equal(-2, m[1, 1]);
		}

		[Fact(DisplayName = "A 3x3 matrix ought to be representable.")]
		public void Can_construct_3x3_matrix()
		{
			var m = new Matrix(new[,] {
				{ -3f, 5, 0 },
				{ 1, -2, -7 },
				{ 0, 1, 1 }
			});
			Assert.Equal(-3, m[0, 0]);
			Assert.Equal(-2, m[1, 1]);
			Assert.Equal(1, m[2, 2]);
		}

		[Fact(DisplayName = "Matrix equality with identical matrices.")]
		public void Can_test_for_equality()
		{
			var a = new Matrix(new[,] {
				{ 1f, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 10, 11, 12 },
				{ 13, 14, 15, 16 }
			});
			var b = new Matrix(new[,] {
				{ 1f, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 10, 11, 12 },
				{ 13, 14, 15, 16 }
			});
			Assert.Equal(a, b);
		}

		[Fact(DisplayName = "Matrix equality with different matrices.")]
		public void Can_test_for_inequality()
		{
			var a = new Matrix(new[,] {
				{ 1f, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 8, 7, 6 },
				{ 5, 4, 3, 2 }
			});
			var b = new Matrix(new[,] {
				{ 2f, 3, 4, 5 },
				{ 6, 7, 8, 9 },
				{ 8, 7, 6, 5 },
				{ 4, 3, 2, 1 }
			});
			Assert.NotEqual(a, b);
		}

		[Fact(DisplayName = "Multiplying two matrices.")]
		public void Can_multiply_matrices()
		{
			var a = new Matrix(new[,] {
				{ 1f, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 8, 7, 6 },
				{ 5, 4, 3, 2 }
			});
			var b = new Matrix(new[,] {
				{ -2f, 1, 2, 3 },
				{ 3, 2, 1, -1 },
				{ 4, 3, 6, 5 },
				{ 1, 2, 7, 8 }
			});
			Assert.Equal(new Matrix(new[,] {
				{ 20f, 22, 50, 48 },
				{ 44, 54, 114, 108 },
				{ 40, 58, 110, 102 },
				{ 16, 26, 46, 42 }
			}), a * b);
		}

		[Fact(DisplayName = "A matrix mupltiplied by a tuple.")]
		public void Can_multiply_matrix_and_tuple()
		{
			var a = new Matrix(new[,] {
				{ 1f, 2, 3, 4 },
				{ 2, 4, 4, 2 },
				{ 8, 6, 4, 1 },
				{ 0, 0, 0, 1 }
			});
			var b = new Tuple(1, 2, 3, 1);
			Assert.Equal(new Tuple(18, 24, 33, 1), a * b);
		}

		[Fact(DisplayName = "Multiplying a matrix by the identity matrix.")]
		public void Can_multiply_matrix_by_identity()
		{
			var a = new Matrix(new[,] {
				{ 0f, 1, 2, 4 },
				{ 1, 2, 4, 8 },
				{ 2, 4, 8, 16 },
				{ 4, 8, 16, 32 }
			});
			Assert.Equal(a, a * Matrix.Identity());
		}

		[Fact(DisplayName = "Multiplying a tuple by the identity matrix.")]
		public void Can_multiply_tuple_by_identity()
		{
			var a = new Tuple(1, 2, 3, 4);
			Assert.Equal(a, Matrix.Identity() * a);
		}

		[Fact(DisplayName = "Transposing a matrix.")]
		public void Can_transpose()
		{
			var a = new Matrix(new[,] {
				{ 0f, 9, 3, 0 },
				{ 9, 8, 0, 8 },
				{ 1, 8, 5, 3 },
				{ 0, 0, 5, 8 }
			});

			Assert.Equal(new Matrix(new[,] {
				{ 0f, 9, 1, 0 },
				{ 9, 8, 8, 0 },
				{ 3, 0, 5, 5 },
				{ 0, 8, 3, 8 }
			}), a.Transpose());
		}

		[Fact(DisplayName = "Transposing the identity matrix.")]
		public void Can_transpose_identity()
		{
			var a = Matrix.Identity();
			Assert.Equal(Matrix.Identity(), a.Transpose());
		}

		[Fact(DisplayName = "Calculating the determinate of a 2x2 matrix.")]
		public void Can_determine_2x2()
		{
			var a = new Matrix(new[,] {
				{ 1f, 5 },
				{ -3, 2 }
			});
			Assert.Equal(17, a.Determinant());
		}

		[Fact(DisplayName = "A submatrix of a 3x3 matrix is a 2x2 matrix.")]
		public void Can_submatrix_3x3()
		{
			var a = new Matrix(new[,]
			{
				{ 1f, 5, 0 },
				{ -3, 2, 7 },
				{ 0, 6, -3 }
			});
			Assert.Equal(new Matrix(new[,] {
				{ -3f, 2 },
				{ 0, 6 }
			}), a.Submatrix(0, 2));
		}

		[Fact(DisplayName = "A submatrix of a 3x4 matrix is a 3x3 matrix.")]
		public void Can_submatrix_4x4()
		{
			var a = new Matrix(new[,] {
				{ -6f, 1, 1, 6 },
				{ -8, 5, 8, 6 },
				{ -1, 0, 8, 2 },
				{ -7, 1, -1, 1 }
			});
			Assert.Equal(new Matrix(new[,] {
				{ -6f, 1, 6 },
				{ -8, 8, 6 },
				{ -7, -1, 1 }
			}), a.Submatrix(2, 1));
		}

		[Fact(DisplayName = "Calculating a minor of a 3x3 matrix.")]
		public void Can_minor_3x3()
		{
			var a = new Matrix(new[,] {
				{ 3f, 5, 0 },
				{ 2, -1, -7 },
				{ 6, -1, 5 }
			});
			var b = a.Submatrix(1, 0);
			Assert.Equal(25, b.Determinant());
			Assert.Equal(25, a.Minor(1, 0));
		}

		[Fact(DisplayName = "Calculating a cofactor of a 3x3 matrix.")]
		public void Can_cofactor_3x3()
		{
			var a = new Matrix(new[,] {
				{ 3f, 5, 0 },
				{ 2, -1, -7 },
				{ 6, -1, 5 }
			});

			Assert.Equal(-12, a.Minor(0, 0));
			Assert.Equal(-12, a.Cofactor(0, 0));
			Assert.Equal(25, a.Minor(1, 0));
			Assert.Equal(-25, a.Cofactor(1, 0));
		}

		[Fact(DisplayName = "Calculating the determinant of a 3x3 matrix.")]
		public void Can_determine_3x3()
		{
			var a = new Matrix(new[,] {
				{ 1f, 2, 6 },
				{ -5, 8, -4 },
				{ 2, 6, 4 }
			});

			Assert.Equal(56, a.Cofactor(0, 0));
			Assert.Equal(12, a.Cofactor(0, 1));
			Assert.Equal(-46, a.Cofactor(0, 2));
			Assert.Equal(-196, a.Determinant());
		}

		[Fact(DisplayName = "Calculating the determinant of a 4x4 matrix.")]
		public void Can_determine_4x4()
		{
			var a = new Matrix(new[,] {
				{ -2f, -8, 3, 5 },
				{ -3, 1, 7, 3 },
				{ 1, 2, -9, 6 },
				{ -6, 7, 7, -9 }
			});

			Assert.Equal(690, a.Cofactor(0, 0));
			Assert.Equal(447, a.Cofactor(0, 1));
			Assert.Equal(210, a.Cofactor(0, 2));
			Assert.Equal(51, a.Cofactor(0, 3));
			Assert.Equal(-4071, a.Determinant());
		}

		[Fact(DisplayName = "Testing an invertible matrix for invertibility.")]
		public void Is_matrix_invertible()
		{
			var a = new Matrix(new[,] {
				{ 6f, 4, 4, 4 },
				{ 5, 5, 7, 6 },
				{ 4, -9, 3, -7 },
				{ 9, 1, 7, -6 }
			});
			Assert.Equal(-2120, a.Determinant());
			Assert.True(a.IsInvertible());
		}

		[Fact(DisplayName = "Testing a noninvertible matrix for invertibility.")]
		public void Is_matrix_noninvertible()
		{
			var a = new Matrix(new[,] {
				{ -4f, 2, -2, -3 },
				{ 9, 6, 2, 6 },
				{ 0, -5, 1, -5 },
				{ 0, 0, 0, 0 }
			});
			Assert.Equal(0, a.Determinant());
			Assert.False(a.IsInvertible());
		}

		[Fact(DisplayName = "Calculating the inverse of a matrix.")]
		public void Can_calculate_inverse()
		{
			var a = new Matrix(new[,] {
				{ -5f, 2, 6, -8 },
				{ 1, -5, 1, 8 },
				{ 7, 7, -6, -7 },
				{ 1, -3, 7, 4 }
			});
			var b = a.Inverse();
			Assert.Equal(532, a.Determinant());
			Assert.Equal(-160, a.Cofactor(2, 3));
			Assert.Equal(-160f / 532, b[3, 2]);
			Assert.Equal(105, a.Cofactor(3, 2));
			Assert.Equal(105f / 532, b[2, 3]);
			Assert.Equal(new Matrix(new[,] {
				{  0.21805f,  0.45113f,  0.24060f, -0.04511f },
				{ -0.80827f, -1.45677f, -0.44361f,  0.52068f },
				{ -0.07895f, -0.22368f, -0.05263f,  0.19737f },
				{ -0.52256f, -0.81391f, -0.30075f,  0.30639f }
			}), b);
		}

		[Fact(DisplayName = "Calculating the inverse of another matrix.")]
		public void Can_calculate_inverse_v2()
		{
			var a = new Matrix(new[,] {
				{ 8f, -5, 9, 2 },
				{ 7, 5, 6, 1 },
				{ -6, 0, 9, 6 },
				{ -3, 0, -9, -4 }
			});

			Assert.Equal(new Matrix(new[,] {
				{ -0.15385f, -0.15385f, -0.28205f, -0.53846f },
				{ -0.07692f,  0.12308f,  0.02564f,  0.03077f },
				{  0.35897f,  0.35897f,  0.43590f,  0.92308f },
				{ -0.69231f, -0.69231f, -0.76923f, -1.92308f }
			}), a.Inverse());
		}

		[Fact(DisplayName = "Calculating the inverse of a third matrix.")]
		public void Can_calculate_inverse_v3()
		{
			var a = new Matrix(new[,] {
				{ 9f, 3, 0, 9 },
				{ -5, -2, -6, -3 },
				{ -4, 9, 6, 4 },
				{ -7, 6, 6, 2 }
			});

			Assert.Equal(new Matrix(new[,] {
				{ -0.04074f, -0.07778f,  0.14444f, -0.22222f },
				{ -0.07778f,  0.03333f,  0.36667f, -0.33333f },
				{ -0.02901f, -0.14630f, -0.10926f,  0.12963f },
				{  0.17778f,  0.06667f, -0.26667f,  0.33333f }
			}), a.Inverse());
		}

		[Fact(DisplayName = "Multiplying a product by its inverse.")]
		public void Can_inverse_undo_product()
		{
			var a = new Matrix(new[,] {
				{ 3f, -9, 7, 3 },
				{ 3, -8, 2, -9 },
				{ -4, 4, 4, 1 },
				{ -6, 5, -1, 1 }
			});

			var b = new Matrix(new[,] {
				{ 8f, 2, 2, 2 },
				{ 3, -1, 7, 0 },
				{ 7, 0, 5, 4 },
				{ 6, -2, 0, 5 }
			});

			var c = a * b;

			Assert.Equal(a, c * b.Inverse());
		}
	}
}
