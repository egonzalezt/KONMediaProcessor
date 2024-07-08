namespace KONMediaProcessor.AudioProcessor.AudioInfo;

using Entities;

public interface IAudioInfoProcessor
{
    AudioInfo GetAudioInfo(string inputFile);
}
