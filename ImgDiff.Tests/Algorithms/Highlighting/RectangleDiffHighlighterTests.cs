using ImgDiff.Algorithms.Highlighting;
using NUnit.Framework;
using System.Drawing;
using ImgDiff.Common;
using Moq;
using System.Threading.Tasks;
using ImgDiff.Algorithms.PixelBuffer;

namespace ImgDiff.Tests.Algorithms.Highlighting
{
    [TestFixture]
    public class RectangleDiffHighlighterTests
    {
        private RectangleDiffHighlighter _testClass;
        private IDiffProgressObserver _progressObserver;

        [SetUp]
        public void SetUp()
        {
            _testClass = new RectangleDiffHighlighter();
            _progressObserver = new Mock<IDiffProgressObserver>().Object;
        }

        [Test]
        public void Highlight_Call()
        {
            using var bmp = new Bitmap(100, 100);
            using var pixels = new PooledPixelBuffer(null);

            pixels.Add(Pixel.Get(0, 0));
            pixels.Add(Pixel.Get(0, 1));
            pixels.Add(Pixel.Get(1, 1));
            pixels.Add(Pixel.Get(1, 0));

            _testClass.Highlight(bmp, pixels, int.MaxValue, _progressObserver);
        }

        [Test]
        public async Task HighlightAsync_Call()
        {
            using var bmp = new Bitmap(100, 100);
            using var pixels = new PooledPixelBuffer(null);

            pixels.Add(Pixel.Get(0, 0));
            pixels.Add(Pixel.Get(0, 1));
            pixels.Add(Pixel.Get(1, 1));
            pixels.Add(Pixel.Get(1, 0));

            await _testClass.HighlightAsync(bmp, pixels, int.MaxValue, _progressObserver);
        }
    }
}
