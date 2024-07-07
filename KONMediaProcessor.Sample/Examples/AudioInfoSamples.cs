namespace KONMediaProcessor.Sample.Examples;

using AudioProcessor.AudioInfo;
using Microsoft.Extensions.Logging;

public class AudioInfoSamples(IAudioInfoProcessor audioInfoProcessor, ILogger<AudioInfoSamples> logger)
{
    const string sampleAudioPath = "Examples/Multimedia/745016__solarpsychedelic__peaceful-chords.wav";
    const string sampleVideoPath = "Examples/Multimedia/sampleVideo.mp4";

    public void GetAudioInfo()
    {
        var response = audioInfoProcessor.GetAudioInfo(sampleAudioPath);
        logger.LogInformation("Audio result Codec: {Codec}, SampleRate: {SampleRate}, Channels: {Channels}",
            response.Codec, response.SampleRate, response.Channels);
    }

    public void GetAudioInfoFromVideo()
    {
        var response = audioInfoProcessor.GetAudioInfo(sampleVideoPath);
        logger.LogInformation("Audio result Codec: {Codec}, SampleRate: {SampleRate}, Channels: {Channels}",
            response.Codec, response.SampleRate, response.Channels);
    }
}
