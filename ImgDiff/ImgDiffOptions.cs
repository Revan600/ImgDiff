using ImgDiff.Algorithms;
using ImgDiff.Algorithms.Highlighting;
using ImgDiff.Algorithms.PixelDelta;
using ImgDiff.Preprocessing;

using System;
using System.IO;

namespace ImgDiff
{
    /// <summary>
    /// Options for setting up <see cref="ImgDiffCalculator"/>
    /// </summary>
    public class ImgDiffOptions : IDisposable
    {
        /// <summary>
        /// Left image file path
        /// </summary>
        public string LeftFile { get; set; }

        /// <summary>
        /// Right image file path
        /// </summary>
        public string RightFile { get; set; }

        /// <summary>
        /// Instance of <see cref="IDiffAlgorithm"/>. Default value is <see cref="DefaultDiffAlgorithm{TDelta, TPreprocessor}"/>
        /// </summary>
        public IDiffAlgorithm DiffAlgorithm { get; set; }

        /// <summary>
        /// Instance of <see cref="IDiffHighlighter"/>. Default value is <see cref="RectangleDiffHighlighter"/>
        /// </summary>
        public IDiffHighlighter Highlighter { get; set; }

        /// <summary>
        /// Instance of <see cref="IDiffProgressObserver"/>. Default value is null
        /// </summary>
        public IDiffProgressObserver ProgressObserver { get; set; }

        public ImgDiffOptions()
        {
            DiffAlgorithm = new DefaultDiffAlgorithm<ArgbPixelDelta, MedianPreprocessor>();
            Highlighter = new RectangleDiffHighlighter();
        }

        public ImgDiffOptions(string leftFile, string rightFile) : this()
        {
            LeftFile = leftFile;
            RightFile = rightFile;
        }

        internal void Validate()
        {
            if (!File.Exists(LeftFile))
                throw new FileNotFoundException($"File {LeftFile} was not found!");

            if (!File.Exists(RightFile))
                throw new FileNotFoundException($"File {LeftFile} was not found!");
        }

        public void Dispose()
        {
            DiffAlgorithm?.Dispose();
            Highlighter?.Dispose();
        }
    }
}
