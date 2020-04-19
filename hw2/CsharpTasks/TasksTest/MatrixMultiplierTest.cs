using NUnit.Framework;
using MatrixMultiplier;
using Matrix;
using System;

namespace TasksTest
{
    [TestFixture]
    public class MatrixMultiplierTest
    {
        private Matrix<Natural> toNaturlMatrix(int[][] intTable) {
            Natural[][] result = new Natural[intTable.Length][];
            int i, j;
            for (i = 0; i < intTable.Length; ++i) {
                result[i] = new Natural[intTable[i].Length];
                for (j = 0; j < intTable[i].Length; ++j)
                    if (intTable[i][j] > 0)
                        result[i][j] = new Natural((uint)intTable[i][j]);
                    else throw new ArgumentException();
            }
            return new Matrix<Natural>(result);
        }

        [Test]
        public void TestCommonCase() {
            int[][] t1 = new int[][] {
                new int[] {1, 2, 3},
                new int[] {4, 5, 6}
            },
            t2 = new int[][] {
                new int[] {1, 2, 3},
                new int[] {4, 5, 6},
                new int[] {7, 8, 9}
            };
            Matrix<Natural> m1 = this.toNaturlMatrix(t1),
                m2 = this.toNaturlMatrix(t2);
            Matrix<Natural> m = Matrix<Natural>.Multiply(m1, m2, new NaturalSemiring());
            int[][] expected = new int[][] {
                new int[] {30, 36, 42},
                new int[] {66, 81, 96}
            };
            int i, j;
            for (i = 0; i < expected.Length; ++i)
                for (j = 0; j < expected[0].Length; ++j)
                    Assert.AreEqual(m[i, j].value, expected[i][j]);
        }

        [Test]
        public void TestWrongSizes() {
            int[][] t1 = new int[][] {
                new int[] {1, 2, 3},
                new int[] {4, 5, 6}
            },
            t2 = new int[][] {
                new int[] {1, 2, 3},
                new int[] {4, 5, 6},
                new int[] {7, 8, 9}
            };
            Matrix<Natural> m1 = this.toNaturlMatrix(t1),
                m2 = this.toNaturlMatrix(t2);
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural>.Multiply(m2, m1, new NaturalSemiring());
                });
        }

        [Test]
        public void TestMatrixWithoutRows() {
            int[][] t = new int[][] { };
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural> m = this.toNaturlMatrix(t);
                });
        }

        [Test]
        public void TestMatrixWithoutColumnsMultiplication()
        {
            int[][] t = new int[][] {
                new int[] { },
                new int[] { }
            };
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural> m = this.toNaturlMatrix(t);
                });
        }

        [Test]
        public void TestInvalidMatrixMultiplication()
        {
            int[][] t = new int[][] {
                new int[] {1},
                new int[] {1, 2}
            };
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural> m = this.toNaturlMatrix(t);
                });
        }
    }
}