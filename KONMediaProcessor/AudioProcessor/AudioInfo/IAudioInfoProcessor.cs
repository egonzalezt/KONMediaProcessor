namespace KONMediaProcessor.AudioProcessor.AudioInfo;

using Domain.AudioInfo;

public interface IAudioInfoProcessor
{
    AudioInfo GetAudioInfo(string inputFile, CancellationToken cancellationToken = default);
}
