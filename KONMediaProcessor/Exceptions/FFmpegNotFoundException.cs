namespace KONMediaProcessor.Exceptions;

public class FFmpegNotFoundException : FFmpegException
{
    public FFmpegNotFoundException() : base() { }

    public FFmpegNotFoundException(string message) : base(message) { }

    public FFmpegNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
