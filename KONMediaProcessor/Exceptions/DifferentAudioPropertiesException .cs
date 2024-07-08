namespace KONMediaProcessor.Exceptions;

public class DifferentAudioPropertiesException : FFmpegException
{
    public DifferentAudioPropertiesException() : base() { }

    public DifferentAudioPropertiesException(string message) : base(message) { }

    public DifferentAudioPropertiesException(string message, Exception innerException) : base(message, innerException) { }
}
