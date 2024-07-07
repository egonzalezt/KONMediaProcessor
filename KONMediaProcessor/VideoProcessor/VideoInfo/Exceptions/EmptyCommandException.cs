namespace KONMediaProcessor.Domain.Exceptions;

public class EmptyCommandException : FFmpegException
{
    public EmptyCommandException() : base() { }

    public EmptyCommandException(string message) : base(message) { }

    public EmptyCommandException(string message, Exception innerException) : base(message, innerException) { }
}
