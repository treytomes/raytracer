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
	}
}
