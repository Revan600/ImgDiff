using ImgDiff.Algorithms;
using ImgDiff.Algorithms.Highlighting;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("ImgDiff.Tests")]

namespace ImgDiff
{
    /// <summary>
    /// Image differences calculator
    /// </summary>
    public class ImgDiffCalculator : IDisposable
    {
        private readonly string _leftPath;
        private readonly string _rightPath;

        private readonly Bitmap _left;
        private readonly Bitmap _right;

        private readonly IDiffAlgorithm _diffAlgorithm;
        private readonly IDiffHighlighter _diffHighlighter;
        private readonly IDiffProgressObserver _progressObserver;

        private ImgDiffCalculator(ImgDiffOptions options)
        {
            _leftPath = options.LeftFile;
            _rightPath = options.RightFile;

            _left = Image.FromFile(_leftPath) as Bitmap;
            _right = Image.FromFile(_rightPath) as Bitmap;

            if (_left == null || _right == null)
                throw new NotSupportedException("Image format not supported!");

            if (_left.Size != _right.Size)
                throw new NotSupportedException("Different image sizes are not supported!");

            _diffAlgorithm = options.DiffAlgorithm;
            _diffHighlighter = options.Highlighter;
            _progressObserver = options.ProgressObserver;
        }

        /// <summary>
        /// Create <see cref="ImgDiffCalculator"/> with <see cref="ImgDiffOptions"/> options
        /// </summary>
        /// <param name="options">Valid <see cref="ImgDiffOptions"/> options</param>
        /// <returns>Instance of <see cref="ImgDiffCalculator"/></returns>
        public static ImgDiffCalculator Create(ImgDiffOptions options)
        {
            options.Validate();

            return new ImgDiffCalculator(options);
        }

        /// <summary>
        /// Calculate difference between provided images
        /// </summary>
        /// <param name="errorTolerance">Maximum allowed pixel delta</param>
        /// <param name="maxDifferences">Maximum differences to recognize</param>
        /// <returns>Image with highlighted differences</returns>
        public Image Calculate(double errorTolerance, int maxDifferences = int.MaxValue)
        {
            using var leftCopy = new Bitmap(_left);
            using var rightCopy = new Bitmap(_right);

            var leftBits = leftCopy.LockBits(
                new Rectangle(0, 0, leftCopy.Width, leftCopy.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            var rightBits = rightCopy.LockBits(
                new Rectangle(0, 0, rightCopy.Width, rightCopy.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            if (Preprocess(leftBits, rightBits) && _progressObserver != null)
            {
                _progressObserver.Report(leftCopy, "leftPreprocess");
                _progressObserver.Report(rightCopy, "rightPreprocess");
            }

            using var pixels = _diffAlgorithm.GetDifferentPixels(errorTolerance, leftBits, rightBits, _progressObserver);

            leftCopy.UnlockBits(leftBits);
            rightCopy.UnlockBits(rightBits);

            var result = new Bitmap(_left);
            _diffHighlighter?.Highlight(result, pixels, maxDifferences, _progressObserver);

            return result;
        }

        /// <summary>
        /// Asynchronously calculate difference between provided images
        /// </summary>
        /// <param name="errorTolerance">Maximum allowed pixel delta</param>
        /// <param name="maxDifferences">Maximum differences to recognize</param>
        /// <returns>Image with highlighted differences</returns>
        public async Task<Image> CalculateAsync(double errorTolerance, int maxDifferences = int.MaxValue)
        {
            using var leftCopy = new Bitmap(_left);
            using var rightCopy = new Bitmap(_right);

            var leftBits = leftCopy.LockBits(
                new Rectangle(0, 0, leftCopy.Width, leftCopy.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            var rightBits = rightCopy.LockBits(
                new Rectangle(0, 0, rightCopy.Width, rightCopy.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            if (await PreprocessAsync(leftBits, rightBits) && _progressObserver != null)
            {
                await _progressObserver.ReportAsync(leftCopy, "leftPreprocess");
                await _progressObserver.ReportAsync(rightCopy, "rightPreprocess");
            }

            var pixels = await _diffAlgorithm.GetDifferentPixelsAsync(errorTolerance, leftBits, rightBits, _progressObserver);

            leftCopy.UnlockBits(leftBits);
            rightCopy.UnlockBits(rightBits);

            var result = new Bitmap(_left);

            if (_diffHighlighter != null)
                await _diffHighlighter.HighlightAsync(result, pixels, maxDifferences, _progressObserver);

            return result;
        }

        private bool Preprocess(BitmapData left, BitmapData right)
        {
            var proc = _diffAlgorithm.ImagePreprocessor;

            proc.Process(left);
            proc.Process(right);

            return true;
        }

        private async Task<bool> PreprocessAsync(BitmapData left, BitmapData right)
        {
            var proc = _diffAlgorithm.ImagePreprocessor;

            await proc.ProcessAsync(left);
            await proc.ProcessAsync(right);

            return true;
        }

        public void Dispose()
        {
            _left?.Dispose();
            _right?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
