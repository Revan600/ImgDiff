using ImgDiff.Common;

using System;

namespace ImgDiff.Algorithms.PixelDelta
{
    /// <summary>
    /// Hsv pixel delta
    /// </summary>
    public class HsvPixelDelta : IPixelDeltaAlgorithm
    {
        public double GetPixelDelta(in Bgra left, in Bgra right)
        {
            var leftHsv = RgbToHsv(left);
            var rightHsv = RgbToHsv(right);

            return Math.Sqrt(
                Math.Pow(leftHsv[0] - rightHsv[0], 2) +
                Math.Pow(leftHsv[1] - rightHsv[1], 2) +
                Math.Pow(leftHsv[2] - rightHsv[2], 2));
        }

        private static double[] RgbToHsv(in Bgra rgb)
        {
            var min = (double)Math.Min(Math.Min(rgb.R, rgb.G), rgb.B);
            var v = (double)Math.Max(Math.Max(rgb.R, rgb.G), rgb.B);
            var delta = v - min;

            var h = 0.0;
            var s = v == 0.0 ? 0.0 : delta / v;

            if (s == 0)
                h = 0.0;
            else
            {
                if (rgb.R == v)
                    h = (rgb.G - rgb.B) / delta;
                else if (rgb.G == v)
                    h = 2 + (rgb.B - rgb.R) / delta;
                else if (rgb.B == v)
                    h = 4 + (rgb.R - rgb.G) / delta;

                h *= 60;

                if (h < 0.0)
                    h += 360;
            }

            return new[] { h, s * 100, (v / 255) * 100 };
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
