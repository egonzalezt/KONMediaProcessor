namespace KONMediaProcessor.Exceptions;

public class FFmpegCommandExecutionException : FFmpegException
{
    public FFmpegCommandExecutionException() : base() { }

    public FFmpegCommandExecutionException(string message) : base(message) { }

    public FFmpegCommandExecutionException(string message, Exception innerException) : base(message, innerException) { }
}
