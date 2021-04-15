using ImgDiff.Algorithms.PixelBuffer;
using ImgDiff.Common;

using System.Collections.Generic;
using System.Drawing;

namespace ImgDiff.Progress.Report
{
    /// <summary>
    /// Interface which is used to generate progress report images
    /// </summary>
    internal interface IReportBuilder
    {
        /// <summary>
        /// Add pixels to report
        /// </summary>
        /// <param name="pixels">List of pixels</param>
        /// <param name="color">Color of pixels</param>
        /// <returns>this</returns>
        IReportBuilder AddPixels(List<Pixel> pixels, in Color color);

        /// <summary>
        /// Add pixels to report
        /// </summary>
        /// <param name="pixels">List of pixels</param>
        /// <param name="color">Color of pixels</param>
        /// <returns>this</returns>
        IReportBuilder AddPixels(IPixelBuffer pixels, in Color color);

        /// <summary>
        /// Add rectangles to report
        /// </summary>
        /// <param name="rectangles">List of rectangles</param>
        /// <param name="pen">Pen which will be used to draw rectangles</param>
        /// <returns>this</returns>
        IReportBuilder AddRectangles(List<Rectangle> rectangles, Pen pen);

        /// <summary>
        /// Add single rectangle to report
        /// </summary>
        /// <param name="rectangle">Rectangle</param>
        /// <param name="pen">Pen which will be used to draw rectangle</param>
        /// <returns>this</returns>
        IReportBuilder AddRectangle(in Rectangle rectangle, Pen pen);

        /// <summary>
        /// Build report image
        /// </summary>
        /// <returns>Report image</returns>
        /// <remarks>After this call current instance of <see cref="IReportBuilder"/> should never be used again</remarks>
        Image Build();
    }
}
