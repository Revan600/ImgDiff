using ImgDiff.Common;

using System;

namespace ImgDiff.Algorithms.PixelBuffer
{
    /// <summary>
    /// Pixel buffer interface which is used as placement of List<Pixel>
    /// </summary>
    public interface IPixelBuffer : IDisposable
    {
        /// <summary>
        /// Count of pixels currently in buffer
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Size of inner array
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Access pixels in buffer by index
        /// </summary>
        /// <param name="index">Valid index in range of 0-<see cref="Count"/></param>
        /// <returns>Pixel at index</returns>
        Pixel this[int index] { get; }

        /// <summary>
        /// Add pixel to buffer
        /// </summary>
        /// <param name="px">Pixel to add</param>
        void Add(Pixel px);

        /// <summary>
        /// Remove pixel from buffer by index and dispose it
        /// </summary>
        /// <param name="index">Valid index in range of 0-<see cref="Count"/></param>
        void Remove(int index);
    }
}
