using ImgDiff.Algorithms.Clustering;
using ImgDiff.Algorithms.PixelBuffer;

using System;
using System.Drawing;
using System.Threading.Tasks;

namespace ImgDiff.Algorithms.Highlighting
{
    /// <summary>
    /// Pixel highlighting algorithm interface which is used to highlight different pixels of images
    /// </summary>
    public interface IDiffHighlighter : IDisposable
    {
        /// <summary>
        /// Implementation of <see cref="IPixelClusteringAlgorithm"/>
        /// </summary>
        IPixelClusteringAlgorithm ClusteringAlgorithm { get; }

        /// <summary>
        /// Highlight pixels buffer on target <see cref="Image"/>
        /// </summary>
        /// <param name="target">Image to draw differences on</param>
        /// <param name="pixels">Source pixel buffer</param>
        /// <param name="limit">Maximum differences to highlight</param>
        /// <param name="progress">Instance of <see cref="IDiffProgressObserver"/> for progress reporting</param>
        void Highlight(Image target, IPixelBuffer pixels, int limit, IDiffProgressObserver progress = null);

        /// <summary>
        /// Asynchronously highlight pixels buffer on target <see cref="Image"/>
        /// </summary>
        /// <param name="target">Image to draw differences on</param>
        /// <param name="pixels">Source pixel buffer</param>
        /// <param name="limit">Maximum differences to highlight</param>
        /// <param name="progress">Instance of <see cref="IDiffProgressObserver"/> for progress reporting</param>
        Task HighlightAsync(Image target, IPixelBuffer pixels, int limit, IDiffProgressObserver progress = null);
    }
}
