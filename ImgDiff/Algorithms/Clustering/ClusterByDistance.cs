using ImgDiff.Algorithms.PixelBuffer;
using ImgDiff.Common;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ImgDiff.Algorithms.Clustering
{
    /// <summary>
    /// Simple clustering by distance between cluster rectangle and pixels around it
    /// </summary>
    public class ClusterByDistance : IPixelClusteringAlgorithm
    {
        private int _maxDistance = 4;

        public int Precision
        {
            get => _maxDistance;
            set => _maxDistance = value;
        }

        public IEnumerable<Rectangle> GetClusters(IPixelBuffer pixels, int limit)
        {
            var results = 0;

            while (pixels.Count > 0 && results < limit)
            {
                var px = pixels[^1];
                var topLeft = Pixel.Get(px);
                var bottomRight = Pixel.Get(px);

                pixels.Remove(pixels.Count - 1);

                while (true)
                {
                    var width = Math.Max(bottomRight.X - topLeft.X, 1);
                    var height = Math.Max(bottomRight.Y - topLeft.Y, 1);

                    var cluster = new Rectangle(topLeft.X, topLeft.Y, width, height);

                    if (!ClusterizeNearestPixels(pixels, cluster, ref topLeft, ref bottomRight))
                        continue;

                    results++;
                    yield return cluster;

                    break;
                }
            }
        }

        public async IAsyncEnumerable<Rectangle> GetClustersAsync(IPixelBuffer pixels, int limit)
        {
            foreach (var cluster in GetClusters(pixels, limit))
            {
                yield return cluster;

                await Task.Yield();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ClusterizeNearestPixels(IPixelBuffer pixels, in Rectangle cluster, ref Pixel topLeft, ref Pixel bottomRight)
        {
            var done = true;

            for (var i = pixels.Count - 1; i >= 0; i--)
            {
                var px = pixels[i];

                if (GetDistance(cluster, px) > _maxDistance)
                    continue;

                if (px.X < topLeft.X)
                    topLeft.X = px.X;

                if (px.Y < topLeft.Y)
                    topLeft.Y = px.Y;

                if (px.X > bottomRight.X)
                    bottomRight.X = px.X;

                if (px.Y > bottomRight.Y)
                    bottomRight.Y = px.Y;

                pixels.Remove(i);

                done = false;
            }

            return done;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double GetDistance(in Rectangle rect, in Pixel p)
        {
            var dx = Math.Max(rect.Left - p.X, 0);
            dx = Math.Max(dx, p.X - rect.Right);

            var dy = Math.Max(rect.Top - p.Y, 0);
            dy = Math.Max(dy, p.Y - rect.Bottom);

            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
