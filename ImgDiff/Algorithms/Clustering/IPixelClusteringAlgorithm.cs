using ImgDiff.Algorithms.PixelBuffer;

using System.Collections.Generic;
using System.Drawing;

namespace ImgDiff.Algorithms.Clustering
{
    /// <summary>
    /// Pixel clustering algorithm interface which is used to merge pixels into clusters
    /// for further highlighting
    /// </summary>
    public interface IPixelClusteringAlgorithm
    {
        /// <summary>
        /// Precision of algorithm. Lower - better
        /// </summary>
        int Precision { get; set; }

        /// <summary>
        /// Clusterize pixel buffer and remove all process pixels from it
        /// </summary>
        /// <param name="pixels">Source pixel buffer</param>
        /// <param name="limit">Maximum clusters to return</param>
        /// <returns>Rectangle containing all pixels from cluster</returns>
        IEnumerable<Rectangle> GetClusters(IPixelBuffer pixels, int limit);

        /// <summary>
        /// Asynchronously clusterize pixel buffer and remove all process pixels from it
        /// </summary>
        /// <param name="pixels">Source pixel buffer</param>
        /// <param name="limit">Maximum clusters to return</param>
        /// <returns>Rectangle containing all pixels from cluster</returns>
        IAsyncEnumerable<Rectangle> GetClustersAsync(IPixelBuffer pixels, int limit);
    }
}
