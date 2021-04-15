using ImgDiff.Algorithms.PixelBuffer;
using ImgDiff.Algorithms.PixelDelta;
using ImgDiff.Common;
using ImgDiff.Preprocessing;
using ImgDiff.Progress.Report;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace ImgDiff.Algorithms
{
    /// <summary>
    /// Default generic algorithm for calculating diff of two images
    /// </summary>
    /// <typeparam name="TDelta"></typeparam>
    /// <typeparam name="TPreprocessor"></typeparam>
    public class DefaultDiffAlgorithm<TDelta, TPreprocessor> : IDiffAlgorithm
        where TDelta : IPixelDeltaAlgorithm, new()
        where TPreprocessor : IImagePreprocessor, new()
    {
        public IPixelDeltaAlgorithm PixelDeltaAlgorithm { get; } = new TDelta();

        public IImagePreprocessor ImagePreprocessor { get; } = new TPreprocessor();

        public IPixelBufferPool PixelBufferPool { get; set; }

        public DefaultDiffAlgorithm()
        {
            PixelBufferPool = new DefaultPixelBufferPool<PooledPixelBuffer>(() => new PooledPixelBuffer(PixelBufferPool));
        }

        public unsafe IPixelBuffer GetDifferentPixels(double errorTolerance, BitmapData left, BitmapData right, IDiffProgressObserver progress = null)
        {
            var result = GenerateDiffMap(errorTolerance, left, right);

            if (progress == null)
                return result;

            using var report = new ReportBuilder(left.Width, left.Height).AddPixels(result, Color.Red).Build();
            progress.Report(report, "diffMap");

            return result;
        }

        public async Task<IPixelBuffer> GetDifferentPixelsAsync(double errorTolerance, BitmapData left, BitmapData right, IDiffProgressObserver progress = null)
        {
            var result = GenerateDiffMap(errorTolerance, left, right);

            if (progress == null)
                return result;

            using var report = new ReportBuilder(left.Width, left.Height).AddPixels(result, Color.Red).Build();
            await progress.ReportAsync(report, "diffMap");

            return result;
        }

        private unsafe IPixelBuffer GenerateDiffMap(double errorTolerance, BitmapData left, BitmapData right)
        {
            var result = PixelBufferPool.Rent();

            for (var y = 0; y < left.Height; y++)
            {
                for (var x = 0; x < left.Width; x++)
                {
                    var index = y * left.Stride + x * 4;
                    var data = *(Bgra*)((byte*)left.Scan0.ToPointer() + index);
                    var dataRight = *(Bgra*)((byte*)right.Scan0.ToPointer() + index);

                    var delta = PixelDeltaAlgorithm.GetPixelDelta(data, dataRight);

                    if (delta <= errorTolerance)
                        continue;

                    result.Add(Pixel.Get(x, y));
                }
            }

            return result;
        }

        public void Dispose()
        {
            PixelDeltaAlgorithm.Dispose();
            ImagePreprocessor.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
