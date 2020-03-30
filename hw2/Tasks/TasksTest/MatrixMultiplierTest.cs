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
            Natural[][] t1 = new Natural[][] {
                new Natural[] {new Natural(1), new Natural(2), new Natural(3)},
                new Natural[] {new Natural(4), new Natural(5), new Natural(6)}
            };
            Natural[][] t2 = new Natural[][] {
                new Natural[] {new Natural(1), new Natural(2), new Natural(3)},
                new Natural[] {new Natural(4), new Natural(5), new Natural(6)},
                new Natural[] {new Natural(7), new Natural(8), new Natural(9)}
            };
            Matrix<Natural> m1 = new Matrix<Natural>(t1),
                m2 = new Matrix<Natural>(t2);
            Natural[][] m = Matrix<Natural>.multiply(m1, m2, new NaturalSemiring()).GetTable();
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
            Natural[][] t1 = new Natural[][] {
                new Natural[] {new Natural(1), new Natural(2), new Natural(3)},
                new Natural[] {new Natural(4), new Natural(5), new Natural(6)}
            };
            Natural[][] t2 = new Natural[][] {
                new Natural[] {new Natural(1), new Natural(2), new Natural(3)},
                new Natural[] {new Natural(4), new Natural(5), new Natural(6)},
                new Natural[] {new Natural(7), new Natural(8), new Natural(9)}
            };
            Matrix<Natural> m1 = new Matrix<Natural>(t1),
                m2 = new Matrix<Natural>(t2);
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural>.multiply(m2, m1, new NaturalSemiring());
                    });
        }

        [Test]
        public void TestMatrixWithoutRows() {
            Natural[][] t = new Natural[][] { };
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural> m = new Matrix<Natural>(t);
                });
        }

        [Test]
        public void TestMatrixWithoutColumnsMultiplication()
        {
            Natural[][] t = new Natural[][] { 
                new Natural[] { },
                new Natural[] { }
            };
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural> m = new Matrix<Natural>(t);
                });
        }

        [Test]
        public void TestInvalidMatrixMultiplication()
        {
            Natural[][] t = new Natural[][] {
                new Natural[] {new Natural(1)},
                new Natural[] {new Natural(1), new Natural(2)}
            };
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural> m = new Matrix<Natural>(t);
                });
        }
    }
}