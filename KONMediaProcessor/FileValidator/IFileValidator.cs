namespace KONMediaProcessor.FileValidator;

public interface IFileValidator
{
    bool FileExists(string filePath);
    void EnsureDirectoryExists(string directoryPath);
    void EnsureDirectoryPathExists(string filePath);
    void ValidatePaths(string[] inputs, string output, bool overrideFile);
}