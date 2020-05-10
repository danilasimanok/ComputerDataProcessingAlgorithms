using NUnit.Framework;
using MatrixMultiplier;

namespace TasksTest
{
    [TestFixture]
    class SerializationTest
    {
        [Test]
        public void TestDeserialization()
        {
            Natural n = new Natural();
            n.FromWord("4");
            Assert.AreEqual(n.value, 4);
        }

        [Test]
        public void TestSerialization()
        {
            Natural n = new Natural(4);
            Assert.AreEqual(n.ToWord(), "4");
        }
    }
}
