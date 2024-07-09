namespace KONMediaProcessor.FileValidator;

using System;
using System.IO;
using Exceptions;

internal class FileValidator : IFileValidator
{
    public bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    public (string[] inputs, string outputPath) ValidatePaths(string[] inputs, string output, bool overrideFile)
    {
        string[] validatedInputs = ValidateInputPaths(inputs);
        string validatedOutput = ValidateOutputPath(output, overrideFile);
        return (validatedInputs, validatedOutput);
    }

    private string[] ValidateInputPaths(string[] inputs)
    {
        if (inputs == null || inputs.Length == 0)
        {
            throw new ArgumentNullException(nameof(inputs), "Input file paths cannot be null or empty.");
        }

        for (int i = 0; i < inputs.Length; i++)
        {
            if (string.IsNullOrEmpty(inputs[i]))
            {
                throw new ArgumentNullException($"Input file path at index {i} is null or empty.");
            }

            inputs[i] = ValidateFileExists(inputs[i]);
        }

        return inputs;
    }

    public string ValidateOutputPath(string output, bool overrideFile)
    {
        if (string.IsNullOrEmpty(output))
        {
            throw new ArgumentNullException(nameof(output), "Output file path cannot be null or empty.");
        }

        output = Path.GetFullPath(output);

        EnsureDirectoryPathExists(output);

        if (!overrideFile && FileExists(output))
        {
            throw new FileAlreadyExistsException(output);
        }

        return output;
    }

    public string ValidateFileExists(string filePath)
    {
        var fileToVerify = Path.GetFullPath(filePath);
        if (!FileExists(fileToVerify))
        {
            throw new FileNotFoundException($"The specified file does not exist: {filePath}");
        }
        return fileToVerify;
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
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentNullException(nameof(filePath), "File path cannot be null or empty.");
        }

        var directoryPath = Path.GetDirectoryName(filePath);
        if (string.IsNullOrEmpty(directoryPath))
        {
            throw new ArgumentNullException(nameof(directoryPath), $"Directory path for {filePath} is null or empty.");
        }

        ValidateDirectoryPermissions(directoryPath);
        EnsureDirectoryExists(directoryPath);
    }

    private void ValidateDirectoryPermissions(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"Directory {directoryPath} not found.");
        }

        if (!HasWritePermission(directoryPath))
        {
            throw new UnauthorizedAccessException($"Insufficient permissions to write to directory {directoryPath}.");
        }
    }

    private bool HasWritePermission(string directoryPath)
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
