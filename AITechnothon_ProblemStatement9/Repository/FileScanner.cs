using AITechnothon_ProblemStatement9.Domain;
using AntiVirus;

namespace AITechnothon_ProblemStatement9.Repository
{
    public class FileScanner : IFileScanner
    {
        public ScanResult ScanFile(string filePath)
        {
            var scanner = new AntiVirus.Scanner();
            ScanResult scanResult = scanner.ScanAndClean(filePath);
            return scanResult;
        }
    }
}
 