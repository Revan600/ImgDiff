using ImgDiff.Common;

using System;

namespace ImgDiff.Algorithms.PixelDelta
{
    /// <summary>
    /// Simple argb pixel delta
    /// </summary>
    public class ArgbPixelDelta : IPixelDeltaAlgorithm
    {
        public double GetPixelDelta(in Bgra left, in Bgra right)
        {
            return Math.Sqrt(
                Math.Pow(left.R - right.R, 2) +
                Math.Pow(left.G - right.G, 2) +
                Math.Pow(left.B - right.B, 2));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
