using System;

namespace ImgDiff.Common
{
    /// <summary>
    /// Pixel coordinate
    /// </summary>
    public class Pixel : IDisposable
    {
        private static readonly SimplePool<Pixel> _pool = new(() => new Pixel(), 128 * 128);

        public int X;
        public int Y;

        private Pixel()
        {
        }
        
        /// <summary>
        /// Get pixel from pool and set it's coordinate
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Rented <see cref="Pixel"/> which should be disposed later</returns>
        public static Pixel Get(int x, int y)
        {
            var result = _pool.Rent();

            result.X = x;
            result.Y = y;

            return result;
        }

        /// <summary>
        /// Get pixel from pool and copy coordinates from <paramref name="px"/>
        /// </summary>
        /// <param name="px"></param>
        /// <returns>Rented <see cref="Pixel"/> which should be disposed later</returns>
        public static Pixel Get(Pixel px)
        {
            var result = _pool.Rent();

            result.X = px.X;
            result.Y = px.Y;

            return result;
        }

        public void Dispose() => _pool.Return(this);
    }
}
