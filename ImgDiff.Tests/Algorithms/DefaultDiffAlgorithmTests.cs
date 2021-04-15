using ImgDiff.Algorithms;
using NUnit.Framework;
using ImgDiff.Algorithms.PixelDelta;
using ImgDiff.Preprocessing;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Moq;

namespace ImgDiff.Tests.Algorithms
{
    [TestFixture]
    public class DefaultDiffAlgorithmTests
    {
        private DefaultDiffAlgorithm<ArgbPixelDelta, EmptyProcessor> _testClass;
        private IDiffProgressObserver _progressObserver;

        private Bitmap leftImg3;
        private Bitmap rightImg3;

        [SetUp]
        public void SetUp()
        {
            _testClass = new DefaultDiffAlgorithm<ArgbPixelDelta, EmptyProcessor>();
            _progressObserver = new Mock<IDiffProgressObserver>().Object;

            leftImg3 = Image.FromFile(Path.Combine("Samples", "Sample_3_A.jpg")) as Bitmap;
            rightImg3 = Image.FromFile(Path.Combine("Samples", "Sample_3_B.jpg")) as Bitmap;
        }

        [Test]
        public void GetDifferentPixels_DifferentImages_ReturnsCorrectValue()
        {
            var leftBits = leftImg3.LockBits(
                new Rectangle(0, 0, leftImg3.Width, leftImg3.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            var rightBits = rightImg3.LockBits(
                new Rectangle(0, 0, rightImg3.Width, rightImg3.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            var pixels = _testClass.GetDifferentPixels(100, leftBits, rightBits, _progressObserver);

            //196 rectangles * 8 height * 8 width
            Assert.AreEqual(196 * 8 * 8, pixels.Count);

            leftImg3.UnlockBits(leftBits);
            rightImg3.UnlockBits(rightBits);
        }

        [Test]
        public void GetDifferentPixels_SameImages_ReturnsZero()
        {
            using var leftCopy = new Bitmap(leftImg3);

            var leftBits = leftImg3.LockBits(
                new Rectangle(0, 0, leftImg3.Width, leftImg3.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            var rightBits = leftCopy.LockBits(
                new Rectangle(0, 0, leftCopy.Width, leftCopy.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            var pixels = _testClass.GetDifferentPixels(100, leftBits, rightBits, _progressObserver);

            Assert.AreEqual(0, pixels.Count);

            leftImg3.UnlockBits(leftBits);
            leftCopy.UnlockBits(rightBits);
        }

        [Test]
        public async Task GetDifferentPixelsAsync_DifferentImages_ReturnsCorrectValue()
        {
            var leftBits = leftImg3.LockBits(
                new Rectangle(0, 0, leftImg3.Width, leftImg3.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            var rightBits = rightImg3.LockBits(
                new Rectangle(0, 0, rightImg3.Width, rightImg3.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            var pixels = await _testClass.GetDifferentPixelsAsync(100, leftBits, rightBits, _progressObserver);

            //196 rectangles * 8 height * 8 width
            Assert.AreEqual(196 * 8 * 8, pixels.Count);

            leftImg3.UnlockBits(leftBits);
            rightImg3.UnlockBits(rightBits);
        }

        [Test]
        public async Task GetDifferentPixelsAsync_SameImages_ReturnsZero()
        {
            using var leftCopy = new Bitmap(leftImg3);

            var leftBits = leftImg3.LockBits(
                new Rectangle(0, 0, leftImg3.Width, leftImg3.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            var rightBits = leftCopy.LockBits(
                new Rectangle(0, 0, leftCopy.Width, leftCopy.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            var pixels = await _testClass.GetDifferentPixelsAsync(100, leftBits, rightBits, _progressObserver);

            Assert.AreEqual(0, pixels.Count);

            leftImg3.UnlockBits(leftBits);
            leftCopy.UnlockBits(rightBits);
        }
    }
}
