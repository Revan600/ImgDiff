using Moq;

using NUnit.Framework;

using System;
using System.IO;
using System.Threading.Tasks;

namespace ImgDiff.Tests
{
    [TestFixture]
    public class ImgDiffCalculatorTests
    {
        private IDiffProgressObserver _progressObserver;

        [SetUp]
        public void SetUp()
        {
            _progressObserver = new Mock<IDiffProgressObserver>().Object;
        }

        [Test]
        public void Create_EmptyOptions_Throws()
        {
            Assert.Throws<FileNotFoundException>(() => ImgDiffCalculator.Create(new ImgDiffOptions()));
        }

        [Test]
        public void Create_NullOptions_Throws()
        {
            Assert.Throws<NullReferenceException>(() => ImgDiffCalculator.Create(null));
        }

        [Test]
        public void Create_DifferentImageSizes_Throws()
        {
            var options = new ImgDiffOptions
            {
                LeftFile = Path.Combine("Samples", "Sample_1_A.jpg"),
                RightFile = Path.Combine("Samples", "Sample_7_B.jpg"),
                ProgressObserver = _progressObserver
            };

            Assert.Throws<NotSupportedException>(() => ImgDiffCalculator.Create(options));
        }

        [Test]
        public void Calculate_Call()
        {
            var options = new ImgDiffOptions
            {
                LeftFile = Path.Combine("Samples", "Sample_1_A.jpg"),
                RightFile = Path.Combine("Samples", "Sample_1_B.jpg"),
                ProgressObserver = _progressObserver
            };

            using var diff = ImgDiffCalculator.Create(options);
            using var img = diff.Calculate(25);
            img.Save("test.jpg");
        }

        [Test]
        public async Task CalculateAsync_Call()
        {
            var options = new ImgDiffOptions
            {
                LeftFile = Path.Combine("Samples", "Sample_1_A.jpg"),
                RightFile = Path.Combine("Samples", "Sample_1_B.jpg"),
                ProgressObserver = _progressObserver
            };

            using var diff = ImgDiffCalculator.Create(options);
            using var img = await diff.CalculateAsync(25);
            img.Save("test.jpg");
        }
    }
}