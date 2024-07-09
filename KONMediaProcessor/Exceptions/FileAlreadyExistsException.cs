namespace KONMediaProcessor.Exceptions;

public class FileAlreadyExistsException : FFmpegException
{
    public FileAlreadyExistsException() : base() { }

    public FileAlreadyExistsException(string message) : base(message) { }

    public FileAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
}

