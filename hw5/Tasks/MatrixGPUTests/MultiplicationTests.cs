using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MatrixGPUTests
{
    [TestClass]
    public class MultiplicationTests
    {
        private static readonly String PLATFORM_NAME = "Intel*";

        public MultiplicationTests() { }

        [TestMethod]
        public void TestMultplicationCommonCase()
        {
            Assert.IsTrue(TestStatements.fstStatement(MultiplicationTests.PLATFORM_NAME));
        }

        [TestMethod]
        public void TestMultplicationExtremeCase()
        {
            Assert.IsTrue(TestStatements.sndStatement(MultiplicationTests.PLATFORM_NAME));
        }
    }
}
