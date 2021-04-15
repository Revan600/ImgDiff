using ImgDiff.Common;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace ImgDiff.Preprocessing
{
    /// <summary>
    /// Preprocessor which applies median (blur) filter to source image
    /// </summary>
    public class MedianPreprocessor : IImagePreprocessor
    {
        private const int Size = 4;

        public void Process(BitmapData imgData)
        {
            var height = imgData.Height;
            var width = imgData.Width;

            var bpp = Image.GetPixelFormatSize(imgData.PixelFormat) / 8;

            for (var xx = 0; xx < width; xx++)
            {
                for (var yy = 0; yy < height; yy++)
                {
                    int avgA = 0, avgR = 0, avgG = 0, avgB = 0;
                    int blurPixelCount = 0;

                    for (var x = xx; x < xx + Size && x < width; x++)
                    {
                        for (var y = yy; y < yy + Size && y < height; y++)
                        {
                            var data = GetPixel(y * imgData.Stride + x * bpp, imgData.Scan0);

                            avgA += data.A;
                            avgR += data.R;
                            avgG += data.G;
                            avgB += data.B;

                            blurPixelCount++;
                        }
                    }

                    avgA /= blurPixelCount;
                    avgR /= blurPixelCount;
                    avgG /= blurPixelCount;
                    avgB /= blurPixelCount;

                    for (var x = xx; x < xx + Size && x < width && x < width; x++)
                    {
                        for (var y = yy; y < yy + Size && y < height && y < height; y++)
                        {
                            SetPixel(new Bgra((byte)avgB, (byte)avgG, (byte)avgR, (byte)avgA),
                                y * imgData.Stride + x * bpp, imgData.Scan0);
                        }
                    }
                }
            }
        }

        public async Task ProcessAsync(BitmapData imgData)
        {
            var height = imgData.Height;
            var width = imgData.Width;

            var bpp = Image.GetPixelFormatSize(imgData.PixelFormat) / 8;

            for (var xx = 0; xx < width; xx++)
            {
                for (var yy = 0; yy < height; yy++)
                {
                    await Task.Yield();

                    int avgA = 0, avgR = 0, avgG = 0, avgB = 0;
                    int blurPixelCount = 0;

                    for (var x = xx; x < xx + Size && x < width; x++)
                    {
                        for (var y = yy; y < yy + Size && y < height; y++)
                        {
                            var data = GetPixel(y * imgData.Stride + x * bpp, imgData.Scan0);

                            avgA += data.A;
                            avgR += data.R;
                            avgG += data.G;
                            avgB += data.B;

                            blurPixelCount++;
                        }
                    }

                    avgA /= blurPixelCount;
                    avgR /= blurPixelCount;
                    avgG /= blurPixelCount;
                    avgB /= blurPixelCount;

                    for (var x = xx; x < xx + Size && x < width && x < width; x++)
                    {
                        for (var y = yy; y < yy + Size && y < height && y < height; y++)
                        {
                            SetPixel(new Bgra((byte)avgB, (byte)avgG, (byte)avgR, (byte)avgA),
                                y * imgData.Stride + x * bpp, imgData.Scan0);
                        }
                    }
                }
            }
        }

        private static unsafe Bgra GetPixel(int index, IntPtr ptr)
            => *(Bgra*)((byte*)ptr.ToPointer() + index);

        private static unsafe void SetPixel(in Bgra color, int index, IntPtr ptr)
            => *(Bgra*)((byte*)ptr.ToPointer() + index) = color;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
