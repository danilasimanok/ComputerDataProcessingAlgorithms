using NUnit.Framework;
using APSP;

namespace TasksTest
{
    [TestFixture]
    class SemigroupTest {

        private StrSemigroup sg;
        private Str a, b, lg;

        public SemigroupTest() {
            this.sg = new StrSemigroup();
            this.a = new Str("a");
            this.b = new Str("b");
            this.lg = new Str("long");
        }

        [Test]
        public void TestInfty() => Assert.AreEqual(this.sg.GetInfty().value, "infty");

        [Test]
        public void TestMultiplication() {
            Assert.AreEqual(this.sg.Multiply(this.a, this.b).value, "ab");
            Assert.AreEqual(this.sg.Multiply(this.a, this.lg).value, "infty");
        }

        public void TestLE() {
            Assert.IsTrue(this.sg.LessOrEqual(this.a, this.b));
            Assert.IsTrue(this.sg.LessOrEqual(this.b, this.b));
            Assert.IsTrue(this.sg.LessOrEqual(this.a, this.lg));
        }
    }
}
