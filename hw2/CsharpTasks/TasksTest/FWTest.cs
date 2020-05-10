using NUnit.Framework;
using APSP;
using Matrix;

namespace TasksTest
{
    [TestFixture]
    class FWTest
    {
        [Test]
        public void TestFW() {
            Matrix<Str> origin = new Matrix<Str>(new Str[][] {
                    new Str[] {new Str("infty"), new Str("bb"), new Str("lool")},
                    new Str[] {new Str("infty"), new Str("infty"), new Str("a")},
                    new Str[] {new Str("lool"), new Str("a"), new Str("infty")}
                }),
                result = FloydWarshallExecutor<Str>.Execute(origin, new StrSemigroup());
            Str[][] expected = new Str[][] {
                new Str[] {new Str("infty"), new Str("bb"), new Str("bba")},
                new Str[] {new Str("infty"), new Str("aa"), new Str("a")},
                new Str[] {new Str("lool"), new Str("a"), new Str("aa")}
            };
            int i, j;
            for (i = 0; i < expected.Length; ++i)
                for (j = 0; j < expected.Length; ++j)
                    Assert.AreEqual(expected[i][j].value, result[i, j].value);
        }
    }
}
