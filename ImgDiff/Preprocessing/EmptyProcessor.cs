using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace ImgDiff.Preprocessing
{
    /// <summary>
    /// Empty image process that does nothing with image
    /// </summary>
    public class EmptyProcessor : IImagePreprocessor
    {
        public void Process(BitmapData imgData) { }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task ProcessAsync(BitmapData imgData) { }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        public void Dispose() { }
    }
}
