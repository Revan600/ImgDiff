using ImgDiff.Algorithms.PixelDelta;
using System;
using NUnit.Framework;
using ImgDiff.Common;

namespace ImgDiff.Tests.Algorithms.PixelDelta
{
    [TestFixture]
    public class HsvPixelDeltaTests
    {
        private HsvPixelDelta _testClass;

        [SetUp]
        public void SetUp()
        {
            _testClass = new HsvPixelDelta();
        }

        [Test]
        public void GetPixelDelta_ValidColors_ReturnsCorrectValue()
        {
            var left = new Bgra(47, 125, 43, 255);
            var right = new Bgra(125, 53, 195, 255);

            var result = _testClass.GetPixelDelta(left, right);

            Assert.IsTrue(Math.Abs(208.5 - result) < 0.1);
        }

        [Test]
        public void GetPixelDelta_SameColors_ReturnsZero()
        {
            var left = new Bgra(0, 0, 0, 255);
            var right = new Bgra(0, 0, 0, 255);

            var result = _testClass.GetPixelDelta(left, right);

            Assert.AreEqual(result, 0.0);
        }
    }
}
