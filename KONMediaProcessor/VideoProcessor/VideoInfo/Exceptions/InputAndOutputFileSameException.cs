namespace KONMediaProcessor.Domain.Exceptions;

public class InputAndOutputFileSameException : FFmpegException
{
    public InputAndOutputFileSameException() : base() { }

    public InputAndOutputFileSameException(string message) : base(message) { }

    public InputAndOutputFileSameException(string message, Exception innerException) : base(message, innerException) { }
}
