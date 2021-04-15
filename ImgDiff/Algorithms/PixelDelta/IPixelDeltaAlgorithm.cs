using ImgDiff.Common;

using System;

namespace ImgDiff.Algorithms.PixelDelta
{
    /// <summary>
    /// Pixel delta algorithm interface which is used to calculate difference between two pixels
    /// </summary>
    public interface IPixelDeltaAlgorithm : IDisposable
    {
        /// <summary>
        /// Calculate difference between two pixels
        /// </summary>
        /// <param name="left">Left pixel color</param>
        /// <param name="right">Right pixel color</param>
        /// <returns>Delta</returns>
        double GetPixelDelta(in Bgra left, in Bgra right);
    }
}
