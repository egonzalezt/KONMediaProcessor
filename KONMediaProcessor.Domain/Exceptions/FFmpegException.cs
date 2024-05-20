namespace KONMediaProcessor.Domain.Exceptions;

public class FFmpegException : Exception
{
    public FFmpegException() : base() { }

    public FFmpegException(string message) : base(message) { }

    public FFmpegException(string message, Exception innerException) : base(message, innerException) { }
}
