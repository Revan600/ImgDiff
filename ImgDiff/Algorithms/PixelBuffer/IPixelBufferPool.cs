namespace ImgDiff.Algorithms.PixelBuffer
{
    /// <summary>
    /// Pool of <see cref="IPixelBuffer"/>
    /// </summary>
    public interface IPixelBufferPool
    {
        /// <summary>
        /// Rent a <see cref="IPixelBuffer"/> instance from pool
        /// </summary>
        /// <returns><see cref="IPixelBuffer"/> instance</returns>
        IPixelBuffer Rent();

        /// <summary>
        /// Return a <see cref="IPixelBuffer"/> instance to pool
        /// </summary>
        /// <param name="buffer">Rented <see cref="IPixelBuffer"/> instance</param>
        void Return(IPixelBuffer buffer);
    }
}
