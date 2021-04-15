using ImgDiff.Common;

using System;

namespace ImgDiff.Algorithms.PixelBuffer
{
    /// <summary>
    /// Simple generic <see cref="IPixelBuffer"/> pool
    /// </summary>
    /// <typeparam name="TBuffer">Implementation of <see cref="IPixelBuffer"/></typeparam>
    public class DefaultPixelBufferPool<TBuffer> : IPixelBufferPool
        where TBuffer : IPixelBuffer
    {
        private readonly SimplePool<TBuffer> _pool;

        public DefaultPixelBufferPool(Func<TBuffer> factory, int defaultCapacity = 0)
        {
            _pool = new SimplePool<TBuffer>(factory, defaultCapacity);
        }

        public IPixelBuffer Rent() => _pool.Rent();

        public void Return(IPixelBuffer buffer)
        {
            if (buffer is not TBuffer b)
                throw new ArgumentException("Invalid buffer type!");

            _pool.Return(b);
        }
    }
}
