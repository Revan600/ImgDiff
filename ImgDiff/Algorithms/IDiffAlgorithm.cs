using ImgDiff.Algorithms.PixelBuffer;
using ImgDiff.Algorithms.PixelDelta;
using ImgDiff.Preprocessing;

using System;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace ImgDiff.Algorithms
{
    /// <summary>
    /// Image different pixels calculation algorithm
    /// </summary>
    public interface IDiffAlgorithm : IDisposable
    {
        /// <summary>
        /// Valid implementation of <see cref="IPixelDeltaAlgorithm"/>
        /// </summary>
        IPixelDeltaAlgorithm PixelDeltaAlgorithm { get; }

        /// <summary>
        /// Implementation of <see cref="IImagePreprocessor"/>. Can be null.
        /// </summary>
        IImagePreprocessor ImagePreprocessor { get; }

        /// <summary>
        /// Calculate difference between two images with error tolerance
        /// </summary>
        /// <param name="errorTolerance">Maximum pixel delta allowed</param>
        /// <param name="left">Left image data</param>
        /// <param name="right">Right image data</param>
        /// <param name="progress">Instance of <see cref="IDiffProgressObserver"/> for progress reporting</param>
        /// <returns><see cref="IPixelBuffer"/> filled with different pixels</returns>
        IPixelBuffer GetDifferentPixels(double errorTolerance, BitmapData left, BitmapData right, IDiffProgressObserver progress = null);

        /// <summary>
        /// Asynchronously calculate difference between two images with error tolerance
        /// </summary>
        /// <param name="errorTolerance">Maximum pixel delta allowed</param>
        /// <param name="left">Left image data</param>
        /// <param name="right">Right image data</param>
        /// <param name="progress">Instance of <see cref="IDiffProgressObserver"/> for progress reporting</param>
        /// <returns><see cref="IPixelBuffer"/> filled with different pixels</returns>
        Task<IPixelBuffer> GetDifferentPixelsAsync(double errorTolerance, BitmapData left, BitmapData right, IDiffProgressObserver progress = null);
    }
}
