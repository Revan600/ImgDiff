using ImgDiff.Algorithms.Clustering;
using ImgDiff.Algorithms.PixelBuffer;
using ImgDiff.Progress.Report;

using System;
using System.Drawing;
using System.Threading.Tasks;

namespace ImgDiff.Algorithms.Highlighting
{
    /// <summary>
    /// Simple algorithm to surround differences by rectangles
    /// </summary>
    public class RectangleDiffHighlighter : IDiffHighlighter
    {
        private readonly Pen _pen;
        private readonly IPixelClusteringAlgorithm _clusteringAlgorithm;

        public IPixelClusteringAlgorithm ClusteringAlgorithm => _clusteringAlgorithm;

        public RectangleDiffHighlighter() : this(new Pen(Color.Red, 1), new ClusterByDistance()) { }

        public RectangleDiffHighlighter(Pen pen, IPixelClusteringAlgorithm clusteringAlgorithm)
        {
            _pen = pen;
            _clusteringAlgorithm = clusteringAlgorithm;
        }

        public void Highlight(Image target, IPixelBuffer pixels, int limit, IDiffProgressObserver progress = null)
        {
            using var g = Graphics.FromImage(target);

            foreach (var cluster in _clusteringAlgorithm.GetClusters(pixels, limit))
            {
                g.DrawRectangle(_pen, cluster);

                if (progress == null)
                    continue;

                using var report = new ReportBuilder(target).AddRectangle(cluster, _pen).Build();
                progress.Report(report, "rect");
            }
        }

        public async Task HighlightAsync(Image target, IPixelBuffer pixels, int limit, IDiffProgressObserver progress = null)
        {
            using var g = Graphics.FromImage(target);

            await foreach (var cluster in _clusteringAlgorithm.GetClustersAsync(pixels, limit))
            {
                g.DrawRectangle(_pen, cluster);

                if (progress == null)
                    continue;

                using var report = new ReportBuilder(target).AddRectangle(cluster, _pen).Build();
                await progress.ReportAsync(report, "rect");
            }
        }

        public void Dispose()
        {
            _pen.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
