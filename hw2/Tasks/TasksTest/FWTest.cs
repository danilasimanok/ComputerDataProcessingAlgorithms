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
            Str[][] origin = new Str[][] {
                new Str[] {new Str("infty"), new Str("bb"), new Str("lool")},
                new Str[] {new Str("infty"), new Str("infty"), new Str("a")},
                new Str[] {new Str("lool"), new Str("a"), new Str("infty")}
            },
            result = FloydWarshallExecutor<Str>.Execute(new Matrix<Str>(origin), new StrSemigroup()).GetTable(),
            pattern = new Str[][] {
                new Str[] {new Str("infty"), new Str("bb"), new Str("bba")},
                new Str[] {new Str("infty"), new Str("aa"), new Str("a")},
                new Str[] {new Str("lool"), new Str("a"), new Str("aa")}
            };
            int i, j;
            for (i = 0; i < pattern.Length; ++i)
                for (j = 0; j < pattern.Length; ++j)
                    Assert.AreEqual(pattern[i][j].value, result[i][j].value);
        }
    }
}
