using System;
using System.Collections.Generic;

namespace ImgDiff.Common
{
    /// <summary>
    /// Simple generic object pool
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>Thread-safe</remarks>
    public class SimplePool<T>
    {
        private readonly Stack<T> _pool;
        private readonly Func<T> _factory;

        private readonly object _lock = new();

        public SimplePool(Func<T> factory, int initialCapacity = 0)
        {
            _factory = factory;
            _pool = new Stack<T>(initialCapacity + (initialCapacity / 2));

            for (int i = 0; i < initialCapacity; i++)
                _pool.Push(_factory());
        }

        /// <summary>
        /// Rent object from the pool
        /// </summary>
        /// <returns>Rented object which should be returned later</returns>
        public T Rent()
        {
            lock (_lock)
            {
                if (_pool.TryPop(out var result))
                    return result;
            }

            return _factory();
        }

        /// <summary>
        /// Return object to the pool
        /// </summary>
        /// <param name="value"></param>
        public void Return(T value)
        {
            lock (_lock)
                _pool.Push(value);
        }
    }
}
