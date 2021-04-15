using System;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace ImgDiff.Preprocessing
{
    /// <summary>
    /// Image preprocessor interface which is used to apply changes on image before further analyzing
    /// in order to reduce errors such as noise
    /// </summary>
    public interface IImagePreprocessor : IDisposable
    {
        /// <summary>
        /// Process image
        /// </summary>
        /// <param name="imgData">Source image data</param>
        void Process(BitmapData imgData);

        /// <summary>
        /// Asynchronously process image
        /// </summary>
        /// <param name="imgData">Source image data</param>
        Task ProcessAsync(BitmapData imgData);
    }
}
