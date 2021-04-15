using ImgDiff.Algorithms.PixelBuffer;
using ImgDiff.Common;

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImgDiff.Progress.Report
{
    /// <summary>
    /// Simple report image builder
    /// </summary>
    internal class ReportBuilder : IReportBuilder
    {
        private readonly Bitmap _result;

        public ReportBuilder(int width, int height)
        {
            _result = new Bitmap(width, height);
        }

        public ReportBuilder(Image src)
        {
            _result = new Bitmap(src);
        }

        public unsafe IReportBuilder AddPixels(List<Pixel> pixels, in Color color)
        {
            var bits = _result.LockBits(new Rectangle(0, 0, _result.Width, _result.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            var redColor = Color.Red.ToArgb();

            foreach (var px in pixels)
            {
                var index = px.Y * bits.Stride + px.X * 4;
                *(int*)((byte*)bits.Scan0.ToPointer() + index) = redColor;
            }

            _result.UnlockBits(bits);

            return this;
        }

        public unsafe IReportBuilder AddPixels(IPixelBuffer pixels, in Color color)
        {
            var bits = _result.LockBits(new Rectangle(0, 0, _result.Width, _result.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            var redColor = Color.Red.ToArgb();

            for (var i = 0; i < pixels.Count; i++)
            {
                var px = pixels[i];

                var index = px.Y * bits.Stride + px.X * 4;
                *(int*)((byte*)bits.Scan0.ToPointer() + index) = redColor;
            }

            _result.UnlockBits(bits);

            return this;
        }

        public IReportBuilder AddRectangles(List<Rectangle> rectangles, Pen pen)
        {
            using (var g = Graphics.FromImage(_result))
                g.DrawRectangles(pen, rectangles.ToArray());

            return this;
        }

        public IReportBuilder AddRectangle(in Rectangle rectangle, Pen pen)
        {
            using (var g = Graphics.FromImage(_result))
                g.DrawRectangle(pen, rectangle);

            return this;
        }

        public Image Build()
        {
            return _result;
        }
    }
}
