using NUnit.Framework;
using System;
using Useless;

namespace TasksTests
{
    [TestFixture]
    public class ResizeArrayTest
    {
        private ResizeArray<int> arr;
        private Random random;

        private void setUpArr() {
            this.arr = new ResizeArray<int>();
            for (int i = 0; i < 100; ++i)
                arr.AddLast(i);
        }

        public ResizeArrayTest() {
            this.setUpArr();
            this.random = new Random();
        }
        
        [TearDown]
        public void Cleanup() => this.setUpArr();

        [Test]
        public void TestGetElements() {
            int index;
            for (int i = 0; i < 50; ++i) {
                index = this.random.Next(0, 100);
                Assert.AreEqual(index, this.arr[index]);
            }
        }

        [Test]
        public void TestSetElements() {
            this.arr[3] = 228;
            Assert.AreEqual(this.arr[3], 228);
        }

        [Test]
        public void TestRange() {
            int i, x;
            for (i = 0; i < 10; ++i) {
                Assert.Throws<IndexOutOfRangeException>(
                    delegate {
                        x = arr[-this.random.Next()];
                    }
                    );
            }
            for (i = 0; i < 10; ++i) {
                Assert.Throws<IndexOutOfRangeException>(
                    delegate {
                        x = arr[100 + this.random.Next()];
                    }
                    );
            }
        }
    }
}