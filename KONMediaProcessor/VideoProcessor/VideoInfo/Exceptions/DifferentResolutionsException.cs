namespace KONMediaProcessor.Domain.Exceptions;

public class DifferentResolutionsException : FFmpegException
{
    public DifferentResolutionsException() : base() { }

    public DifferentResolutionsException(string message) : base(message) { }

    public DifferentResolutionsException(string message, Exception innerException) : base(message, innerException) { }
}
