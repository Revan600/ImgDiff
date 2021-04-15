using System;
using NUnit.Framework;
using ImgDiff.Common;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using ImgDiff.Preprocessing;
using ImgDiff.Algorithms;
using ImgDiff.Algorithms.PixelDelta;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ImgDiff.Algorithms.Clustering;

namespace ImgDiff.Tests.Algorithms.Clustering
{
    [TestFixture]
    public class ClusterByDistanceTests
    {
        private ClusterByDistance _testClass;

        private Bitmap leftImg1;
        private Bitmap rightImg1;

        private Bitmap leftImg3;
        private Bitmap rightImg3;

        private IDiffAlgorithm diff;

        [SetUp]
        public void SetUp()
        {
            _testClass = new ClusterByDistance();

            diff = new DefaultDiffAlgorithm<ArgbPixelDelta, MedianPreprocessor>();

            leftImg1 = Image.FromFile(Path.Combine("Samples", "Sample_1_A.jpg")) as Bitmap;
            rightImg1 = Image.FromFile(Path.Combine("Samples", "Sample_1_B.jpg")) as Bitmap;

            leftImg3 = Image.FromFile(Path.Combine("Samples", "Sample_3_A.jpg")) as Bitmap;
            rightImg3 = Image.FromFile(Path.Combine("Samples", "Sample_3_B.jpg")) as Bitmap;
        }

        [Test]
        public void GetClusters_SameFiles_ReturnsZero()
        {
            using var leftImgCopy1 = new Bitmap(leftImg1);
            using var leftImgCopy2 = new Bitmap(leftImg1);

            var leftBits = leftImgCopy1.LockBits(
                new Rectangle(0, 0, leftImgCopy1.Width, leftImgCopy1.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            var leftBits2 = leftImgCopy2.LockBits(
                new Rectangle(0, 0, leftImgCopy2.Width, leftImgCopy2.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            diff.ImagePreprocessor.Process(leftBits);
            diff.ImagePreprocessor.Process(leftBits2);

            var pixels = diff.GetDifferentPixels(25, leftBits, leftBits2);
            var clusters = _testClass.GetClusters(pixels, int.MaxValue).ToList();

            Assert.AreEqual(0, clusters.Count);

            leftImgCopy1.UnlockBits(leftBits);
            leftImgCopy2.UnlockBits(leftBits2);
        }

        [Test]
        public void GetClusters_DifferentFiles_ReturnsExactValue([Range(0, 8)] int limit)
        {
            var leftBits = leftImg1.LockBits(
                new Rectangle(0, 0, leftImg1.Width, leftImg1.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            var rightBits = rightImg1.LockBits(
                new Rectangle(0, 0, rightImg1.Width, rightImg1.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            diff.ImagePreprocessor.Process(leftBits);
            diff.ImagePreprocessor.Process(rightBits);

            var pixels = diff.GetDifferentPixels(25, leftBits, rightBits);
            var clusters = _testClass.GetClusters(pixels, limit).ToList();

            Assert.AreEqual(limit, clusters.Count);

            leftImg1.UnlockBits(leftBits);
            rightImg1.UnlockBits(rightBits);
        }

        [Test]
        public void GetClusters_DifferentFiles_ReturnsCorrectValue()
        {
            var leftBits = leftImg3.LockBits(
                new Rectangle(0, 0, leftImg3.Width, leftImg3.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            var rightBits = rightImg3.LockBits(
                new Rectangle(0, 0, rightImg3.Width, rightImg3.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            var oldValue = _testClass.Precision;
            _testClass.Precision = 1;

            var pixels = diff.GetDifferentPixels(100, leftBits, rightBits);
            var clusters = _testClass.GetClusters(pixels, int.MaxValue).ToList();

            _testClass.Precision = oldValue;

            Assert.AreEqual(196, clusters.Count);

            leftImg3.UnlockBits(leftBits);
            rightImg3.UnlockBits(rightBits);
        }

        [Test]
        public async Task GetClustersAsync_SameFiles_ReturnsZero()
        {
            using var leftImgCopy1 = new Bitmap(leftImg1);
            using var leftImgCopy2 = new Bitmap(leftImg1);

            var leftBits = leftImgCopy1.LockBits(
                new Rectangle(0, 0, leftImgCopy1.Width, leftImgCopy1.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            var rightBits = leftImgCopy2.LockBits(
                new Rectangle(0, 0, leftImgCopy2.Width, leftImgCopy2.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            await diff.ImagePreprocessor.ProcessAsync(leftBits);
            await diff.ImagePreprocessor.ProcessAsync(rightBits);

            var pixels = await diff.GetDifferentPixelsAsync(25, leftBits, rightBits);
            var clusters = new List<Rectangle>();

            await foreach (var cluster in _testClass.GetClustersAsync(pixels, int.MaxValue))
                clusters.Add(cluster);

            Assert.AreEqual(0, clusters.Count);

            leftImgCopy1.UnlockBits(leftBits);
            leftImgCopy2.UnlockBits(rightBits);
        }

        [Test]
        public async Task GetClustersAsync_DifferentFiles_ReturnsExactValue([Range(0, 8)] int limit)
        {
            var leftBits = leftImg1.LockBits(
                new Rectangle(0, 0, leftImg1.Width, leftImg1.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            var rightBits = rightImg1.LockBits(
                new Rectangle(0, 0, rightImg1.Width, rightImg1.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            await diff.ImagePreprocessor.ProcessAsync(leftBits);
            await diff.ImagePreprocessor.ProcessAsync(rightBits);

            var pixels = await diff.GetDifferentPixelsAsync(25, leftBits, rightBits);
            var clusters = new List<Rectangle>();

            await foreach (var cluster in _testClass.GetClustersAsync(pixels, limit))
                clusters.Add(cluster);

            Assert.AreEqual(limit, clusters.Count);

            leftImg1.UnlockBits(leftBits);
            rightImg1.UnlockBits(rightBits);
        }

        [Test]
        public async Task GetClustersAsync_DifferentFiles_ReturnsCorrectValue()
        {
            var leftBits = leftImg3.LockBits(
                new Rectangle(0, 0, leftImg3.Width, leftImg3.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            var rightBits = rightImg3.LockBits(
                new Rectangle(0, 0, rightImg3.Width, rightImg3.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            var oldValue = _testClass.Precision;
            _testClass.Precision = 1;

            var pixels = await diff.GetDifferentPixelsAsync(25, leftBits, rightBits);
            var clusters = new List<Rectangle>();

            await foreach (var cluster in _testClass.GetClustersAsync(pixels, int.MaxValue))
                clusters.Add(cluster);

            _testClass.Precision = oldValue;

            Assert.AreEqual(196, clusters.Count);

            leftImg3.UnlockBits(leftBits);
            rightImg3.UnlockBits(rightBits);
        }

        [Test]
        public void GetClusters_DifferentFiles_ClearsPixelBuffer()
        {
            var leftBits = leftImg1.LockBits(
                new Rectangle(0, 0, leftImg1.Width, leftImg1.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            var rightBits = rightImg1.LockBits(
                new Rectangle(0, 0, rightImg1.Width, rightImg1.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            diff.ImagePreprocessor.Process(leftBits);
            diff.ImagePreprocessor.Process(rightBits);

            var pixels = diff.GetDifferentPixels(25, leftBits, rightBits);
            var clusters = new List<Rectangle>();

            foreach (var cluster in _testClass.GetClusters(pixels, int.MaxValue))
                clusters.Add(cluster);

            Assert.AreEqual(0, pixels.Count);

            leftImg1.UnlockBits(leftBits);
            rightImg1.UnlockBits(rightBits);
        }

        [Test]
        public async Task GetClustersAsync_DifferentFiles_ClearsPixelBuffer()
        {
            var leftBits = leftImg3.LockBits(
                new Rectangle(0, 0, leftImg3.Width, leftImg3.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            var rightBits = rightImg3.LockBits(
                new Rectangle(0, 0, rightImg3.Width, rightImg3.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            var pixels = await diff.GetDifferentPixelsAsync(25, leftBits, rightBits);
            var clusters = new List<Rectangle>();

            await foreach (var cluster in _testClass.GetClustersAsync(pixels, int.MaxValue))
                clusters.Add(cluster);

            Assert.AreEqual(0, pixels.Count);

            leftImg3.UnlockBits(leftBits);
            rightImg3.UnlockBits(rightBits);
        }
    }
}
