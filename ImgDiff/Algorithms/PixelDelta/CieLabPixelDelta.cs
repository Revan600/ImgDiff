using ImgDiff.Common;

using System;

namespace ImgDiff.Algorithms.PixelDelta
{
    /// <summary>
    /// CieLab pixel delta
    /// </summary>
    public class CieLabPixelDelta : IPixelDeltaAlgorithm
    {
        public double GetPixelDelta(in Bgra left, in Bgra right)
        {
            var leftLab = RgbToLab(left);
            var rightLab = RgbToLab(right);

            return Math.Sqrt(
                Math.Pow(leftLab[0] - rightLab[0], 2) +
                Math.Pow(leftLab[1] - rightLab[1], 2) +
                Math.Pow(leftLab[2] - rightLab[2], 2));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        //http://www.easyrgb.com/en/math.php
        private static double[] RgbToLab(in Bgra color)
        {
            var r = color.R / 255.0;
            var g = color.G / 255.0;
            var b = color.B / 255.0;

            r = ConvertColor(r) * 100.0;
            g = ConvertColor(g) * 100.0;
            b = ConvertColor(b) * 100.0;

            var x = r * 0.4124 + g * 0.3576 + b * 0.1805;
            var y = r * 0.2126 + g * 0.7152 + b * 0.0722;
            var z = r * 0.0193 + g * 0.1192 + b * 0.9505;

            //D65	95.047	100.000	108.883	94.811	100.000	107.304	Daylight, sRGB, Adobe-RGB
            x /= 95.047;
            y /= 100.000;
            z /= 108.883;

            x = ConvertColorXyz(x);
            y = ConvertColorXyz(y);
            z = ConvertColorXyz(z);

            return new[]
            {
                (116.0 * y) - 16.0,
                500.0 * (x - y),
                200.0 * (y - z)
            };
        }

        private static double ConvertColor(double c)
        {
            var result = c;

            if (result > 0.04045)
                result = Math.Pow((result + 0.055) / 1.055, 2.4);
            else
                result /= 12.92;

            return result;
        }

        private static double ConvertColorXyz(double c)
        {
            var result = c;

            if (result > 0.008856)
                result = Math.Pow(result, 1.0 / 3.0);
            else
                result = (7.787 * result) + (16.0 / 116.0);

            return result;
        }
    }
}
