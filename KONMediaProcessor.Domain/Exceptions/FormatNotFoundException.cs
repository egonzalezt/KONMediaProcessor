namespace KONMediaProcessor.Domain.Exceptions;

public class FormatNotFoundException : FFmpegException
{
    public FormatNotFoundException() : base() { }

    public FormatNotFoundException(string message) : base(message) { }

    public FormatNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
