using ImgDiff.Common;

using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace ImgDiff.Algorithms.PixelBuffer
{
    /// <summary>
    /// Simple <see cref="IPixelBuffer"/> implementation with inner array pooling
    /// </summary>
    public class PooledPixelBuffer : IPixelBuffer
    {
        private static readonly ArrayPool<Pixel> _pool = ArrayPool<Pixel>.Shared;

        private const int DefaultCapacity = 16;

        private readonly IPixelBufferPool _bufferPool;

        private int _count;
        private Pixel[] _array;

        public int Count => _count;

        public int Capacity => _array.Length;

        public Pixel this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index >= _count)
                    throw new ArgumentOutOfRangeException($"Index {index} was out of range!");

                return _array[index];
            }
        }

        public PooledPixelBuffer(IPixelBufferPool bufferPool, int capacity = DefaultCapacity)
        {
            _bufferPool = bufferPool;
            _array = capacity == 0 ? _pool.Rent(DefaultCapacity) : _pool.Rent(capacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Pixel px)
        {
            EnsureCapacity(_count + 1);

            _array[_count++] = px;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(int index)
        {
            if (index >= _count)
                throw new ArgumentOutOfRangeException($"Index {index} was out of range!");

            _array[index].Dispose();
            _array[index] = null;

            --_count;

            if (index >= _count)
                return;

            Array.Copy(_array, index + 1, _array, index, _count - index); //compact array
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureCapacity(int size)
        {
            if (_array.Length >= size)
                return;

            var newCapacity = _array.Length == 0 ? DefaultCapacity : _array.Length * 2;

            do
            {
                newCapacity += _array.Length * 2;
            } while (newCapacity < size);

            var newArray = _pool.Rent(newCapacity);
            Array.Copy(_array, newArray, _array.Length);
            _pool.Return(_array);
            _array = newArray;
        }

        public void Dispose()
        {
            for (var i = 0; i < _count; i++)
            {
                _array[i].Dispose();
                _array[i] = null;
            }

            _count = 0;

            GC.SuppressFinalize(this);

            if (_bufferPool == null)
            {
                _pool.Return(_array);
                return;
            }

            _bufferPool.Return(this);
        }
    }
}
