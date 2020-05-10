using NUnit.Framework;
using MatrixMultiplier;

namespace TasksTest
{
    [TestFixture]
    class SemiringTest
    {
        [Test]
        public void TestIdentityElement() =>
            Assert.AreEqual(new NaturalSemiring().GetIdentityElement().value, 0);

        [Test]
        public void TestAddition() {
            Natural n1 = new Natural(1),
                n2 = new Natural(2);
            Assert.AreEqual(new NaturalSemiring().Add(n1, n2).value, 3);
        }

        [Test]
        public void TestMultiplication() {
            Natural n1 = new Natural(2),
                n2 = new Natural(3);
            Assert.AreEqual(new NaturalSemiring().Multiply(n1, n2).value, 6);
        }
    }
}
