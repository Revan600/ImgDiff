using ImgDiff.Progress.Report;
using System;
using NUnit.Framework;
using System.Drawing;
using System.Collections.Generic;
using ImgDiff.Common;

namespace ImgDiff.Tests.Progress.Report
{
    [TestFixture]
    public class ReportBuilderTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void AddRectangle_Call()
        {
            using var pen = new Pen(Color.Red, 1);
            using var _ = new ReportBuilder(100, 100).AddRectangle(new Rectangle(0, 0, 10, 10), pen).Build();
        }

        [Test]
        public void AddRectangles_Call()
        {
            var rectangles = new List<Rectangle>
            {
                new Rectangle(0,0,10,10),
                new Rectangle(20,20,10,10)
            };

            using var pen = new Pen(Color.Red, 1);
            using var _ = new ReportBuilder(100, 100).AddRectangles(rectangles, pen).Build();
        }

        [Test]
        public void AddPixels_Call()
        {
            var pixels = new List<Pixel>
            {
                Pixel.Get(0,0),
                Pixel.Get(0,1),
                Pixel.Get(0,1)
            };

            using var pen = new Pen(Color.Red, 1);
            using var _ = new ReportBuilder(100, 100).AddPixels(pixels, Color.Red).Build();

            foreach (var px in pixels)
                px.Dispose();
        }
    }
}