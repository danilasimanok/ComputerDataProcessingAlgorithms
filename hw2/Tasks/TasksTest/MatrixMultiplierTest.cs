using NUnit.Framework;
using MatrixMultiplier;
using Matrix;
using System;

namespace TasksTest
{
    [TestFixture]
    public class MatrixMultiplierTest
    {
        [Test]
        public void TestCommonCase() {
            Natural[][] m1 = new Natural[][] {
                new Natural[] {new Natural(1), new Natural(2), new Natural(3)},
                new Natural[] {new Natural(4), new Natural(5), new Natural(6)}
            };
            Natural[][] m2 = new Natural[][] {
                new Natural[] {new Natural(1), new Natural(2), new Natural(3)},
                new Natural[] {new Natural(4), new Natural(5), new Natural(6)},
                new Natural[] {new Natural(7), new Natural(8), new Natural(9)}
            };
            Natural[][] m = Matrix<Natural>.multiply(m1, m2, new NaturalSemiring());
            Natural[][] pattern = new Natural[][] {
                new Natural[] {new Natural(30), new Natural(36), new Natural(42)},
                new Natural[] {new Natural(66), new Natural(81), new Natural(96)}
            };
            int i, j;
            for (i = 0; i < pattern.Length; ++i)
                for (j = 0; j < pattern[0].Length; ++j)
                    Assert.AreEqual(m[i][j].value, pattern[i][j].value);
        }

        [Test]
        public void TestWrongSizes() {
            Natural[][] m1 = new Natural[][] {
                new Natural[] {new Natural(1), new Natural(2), new Natural(3)},
                new Natural[] {new Natural(4), new Natural(5), new Natural(6)}
            };
            Natural[][] m2 = new Natural[][] {
                new Natural[] {new Natural(1), new Natural(2), new Natural(3)},
                new Natural[] {new Natural(4), new Natural(5), new Natural(6)},
                new Natural[] {new Natural(7), new Natural(8), new Natural(9)}
            };
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural>.multiply(m2, m1, new NaturalSemiring());
                    });
        }

        [Test]
        public void TestMatrixWithoutRowsMultiplication() {
            Natural[][] m1 = new Natural[][] { };
            Natural[][] m2 = new Natural[][] {
                new Natural[] {new Natural(1), new Natural(2), new Natural(3)},
                new Natural[] {new Natural(4), new Natural(5), new Natural(6)}
            };
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural>.multiply(m2, m1, new NaturalSemiring());
                });
        }

        [Test]
        public void TestMatrixWithoutColumnsMultiplication()
        {
            Natural[][] m1 = new Natural[][] { 
                new Natural[] { },
                new Natural[] { }
            };
            Natural[][] m2 = new Natural[][] {
                new Natural[] {new Natural(1), new Natural(2), new Natural(3)},
                new Natural[] {new Natural(4), new Natural(5), new Natural(6)}
            };
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural>.multiply(m2, m1, new NaturalSemiring());
                });
        }

        [Test]
        public void TestInvalidMatrixMultiplication()
        {
            Natural[][] m1 = new Natural[][] {
                new Natural[] {new Natural(1)},
                new Natural[] {new Natural(1), new Natural(2)}
            };
            Natural[][] m2 = new Natural[][] {
                new Natural[] {new Natural(1), new Natural(2), new Natural(3)},
                new Natural[] {new Natural(4), new Natural(5), new Natural(6)}
            };
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural>.multiply(m2, m1, new NaturalSemiring());
                });
        }
    }
}