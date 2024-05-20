namespace KONMediaProcessor.FileValidator;

using Domain.Exceptions;

internal class FileValidator : IFileValidator
{
    public bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    public void EnsureDirectoryExists(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    public void EnsureDirectoryPathExists(string filePath)
    {
        if (filePath is null || string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        var directoryPath = Path.GetDirectoryName(filePath);
        if (directoryPath is null)
        {
            throw new ArgumentNullException(nameof(directoryPath));
        }
        ValidateDirectoryPermissions(directoryPath);
        EnsureDirectoryExists(directoryPath);
    }

    public void ValidatePaths(string[] inputs, string output, bool overrideFile)
    {
        foreach (var input in inputs)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (string.Equals(input, output, StringComparison.OrdinalIgnoreCase))
            {
                throw new InputAndOutputFileSameException("The input file cannot be the same as the output file.");
            }

            if (!FileExists(input))
            {
                throw new FileNotFoundException(input);
            }
        }

        if (string.IsNullOrEmpty(output))
        {
            throw new ArgumentNullException(nameof(output));
        }

        EnsureDirectoryPathExists(output);
        if (!overrideFile && FileExists(output))
        {
            throw new FileAlreadyExistsException(output);
        }
    }

    private static void ValidateDirectoryPermissions(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"Directory {directoryPath} not found");
        }

        if (!HasWritePermission(directoryPath))
        {
            throw new UnauthorizedAccessException($"The current path {directoryPath} requires permissions to creates a directory");
        }
    }

    private static bool HasWritePermission(string directoryPath)
    {
        try
        {
            string tempFilePath = Path.Combine(directoryPath, Guid.NewGuid().ToString() + ".tmp");
            using (FileStream fs = File.Create(tempFilePath)) { }
            File.Delete(tempFilePath);
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }
}
