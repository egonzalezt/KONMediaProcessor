namespace KONMediaProcessor.Shared.Entities.Color.Exceptions;

using KONMediaProcessor.Exceptions;

public class ColorHexNotValidException : FFmpegException
{
    public ColorHexNotValidException() : base() { }

    public ColorHexNotValidException(string message) : base(message) { }

    public ColorHexNotValidException(string message, Exception innerException) : base(message, innerException) { }
}
