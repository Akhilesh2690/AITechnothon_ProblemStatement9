using AntiVirus;

namespace AITechnothon_ProblemStatement9.Domain
{
    public interface IFileScanner
    {
        ScanResult ScanFile(string filePath);
    }
}