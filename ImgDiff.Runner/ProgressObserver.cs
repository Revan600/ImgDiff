using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace ImgDiff.Runner
{
    /// <summary>
    /// Example of <see cref="IDiffProgressObserver"/>
    /// </summary>
    internal class ProgressObserver : IDiffProgressObserver
    {
        private const string DirectoryName = "Progress";

        private readonly string _baseName;
        private int _stepNumber;

        public ProgressObserver(string baseName)
        {
            _baseName = baseName;

            Directory.CreateDirectory(DirectoryName);
        }

        public void Report(Image img, string stepName)
        {
            img.Save(Path.Combine(DirectoryName, $"{_baseName}_{_stepNumber++}_{stepName}.jpg"));
        }

        public async Task ReportAsync(Image img, string stepName)
        {
            await Task.Factory.StartNew(() => img.Save(Path.Combine(DirectoryName, $"{_baseName}_{_stepNumber++}_{stepName}.jpg")))
                .ConfigureAwait(false);
        }
    }
}
