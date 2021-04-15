using System.IO;

namespace ImgDiff.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            using var options = new ImgDiffOptions
            {
                LeftFile = Path.Combine("Samples", "Sample_1_A.jpg"),
                RightFile = Path.Combine("Samples", "Sample_1_B.jpg"),
                ProgressObserver = new ProgressObserver("Sample_1")
            };

            using var calculator = ImgDiffCalculator.Create(options);
            using var diffImg = calculator.Calculate(30);
            diffImg.Save("test.jpg");
        }
    }
}
