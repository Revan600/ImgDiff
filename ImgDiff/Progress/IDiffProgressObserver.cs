using System.Drawing;
using System.Threading.Tasks;

namespace ImgDiff
{
    /// <summary>
    /// Image diff progress observer interface which is used to report progress of
    /// differences calculation
    /// </summary>
    public interface IDiffProgressObserver
    {
        /// <summary>
        /// This method called on next step of calculation
        /// </summary>
        /// <param name="img">Progress report image</param>
        /// <param name="stepName">Name of the step</param>
        void Report(Image img, string stepName);

        /// <summary>
        /// This method asynchronously called on next step of calculation
        /// </summary>
        /// <param name="img">Progress report image</param>
        /// <param name="stepName">Name of the step</param>
        Task ReportAsync(Image img, string stepName);
    }
}
