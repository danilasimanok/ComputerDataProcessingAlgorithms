using MatrixMultiplier;
using NUnit.Framework;
using System;

namespace TasksTests
{
    [TestFixture]
    class MatrixTest
    {
        private Matrix matrix0, matrix1, matrix2;
        private int[][] table;

        public MatrixTest() {
            int[][] table0 = new int[][] {
                new int[] {3, 2, 2, 0},
                new int[] {2, 2, 3, 0},
                new int[] {3, 2, 2, 1}
            },
            table1 = new int[][] {
                new int[] {3},
                new int[] {2},
                new int[] {1},
                new int[] {3},
            },
            table2 = new int[][] {
                new int[] {15},
                new int[] {13},
                new int[] {18}
            };
            this.table = table0;
            this.matrix0 = new Matrix(table0);
            this.matrix1 = new Matrix(table1);
            this.matrix2 = new Matrix(table2);
        }

        [Test]
        public void TestCorrectMatrixCreation() => Assert.AreEqual(matrix0.GetContent(), this.table);

        [Test]
        public void TestEmptyMatrixCreation() {
            int[][] invalid0 = new int[][] { },
            invalid1 = new int[][] {
                new int[] { },
                new int[] { },
                new int[] { },
                new int[] { }
            };
            Assert.Throws<ArgumentException>(
                delegate {
                    new Matrix(invalid0);
                }
                );
            Assert.Throws<ArgumentException>(
                delegate {
                    new Matrix(invalid1);
                }
                );
        }

        [Test]
        public void TestNotRectangleMatrixCreation() {
            int[][] invalid = new int[][] {
                new int[] {1, 2, 3, 4},
                new int[] {1, 2, 3},
                new int[] {1, 2, 3, 4},
            };
            Assert.Throws<ArgumentException>(
                delegate {
                    new Matrix(invalid);
                }
                );
        }

        [Test]
        public void TestCorrectMultiplication() => Assert.AreEqual(this.matrix0 * this.matrix1, this.matrix2);

        [Test]
        public void TestInvalidMultiplication() =>
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix m = this.matrix1 * this.matrix0;
                }
                );
    }
}