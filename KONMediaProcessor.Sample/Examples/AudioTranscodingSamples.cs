namespace KONMediaProcessor.Sample.Examples;

using AudioProcessor.AudioTranscoding;
using Microsoft.Extensions.Logging;

public class AudioTranscodingSamples
{
    private readonly string _outputPath;
    private readonly IAudioTranscodingProcessor _audioTranscodingProcessor;
    private readonly ILogger<AudioTranscodingSamples> _logger;

    public AudioTranscodingSamples(
        IAudioTranscodingProcessor audioTranscodingProcessor, 
        ILogger<AudioTranscodingSamples> logger
    )
    {
        _logger = logger;
        _audioTranscodingProcessor = audioTranscodingProcessor;
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string className = GetType().Name;
        _outputPath = Path.Combine(desktopPath, nameof(KONMediaProcessor), className);

        if (!Directory.Exists(_outputPath))
        {
            Directory.CreateDirectory(_outputPath);
        }
    }

    public void TranscodeAudio()
    {
        var originalVideoPath = "Examples/Multimedia/744946__crackerjack56__downed.mp3";
        var outputFilePath = Path.Combine(_outputPath, $"Transcode-{Guid.NewGuid()}");
        _audioTranscodingProcessor.TranscodeAudio(originalVideoPath, outputFilePath, Domain.Shared.AudioCodec.FLAC, 256);
        _logger.LogInformation("Final audio located at: {Path}", outputFilePath);
    }

    public void ChangeBitRate()
    {
        var originalVideoPath = "Examples/Multimedia/744946__crackerjack56__downed.mp3";
        var outputFilePath = Path.Combine(_outputPath, $"BitRate-{Guid.NewGuid()}.mp3");
        _audioTranscodingProcessor.ChangeAudioBitrate(originalVideoPath, outputFilePath, 256);
        _logger.LogInformation("Final audio located at: {Path}", outputFilePath);
    }

    public void ChangeChannels()
    {
        var originalVideoPath = "Examples/Multimedia/744946__crackerjack56__downed.mp3";
        var outputFilePath = Path.Combine(_outputPath, $"Channels-{Guid.NewGuid()}.mp3");
        _audioTranscodingProcessor.ChangeAudioChannels(originalVideoPath, outputFilePath, 1);
        _logger.LogInformation("Final audio located at: {Path}", outputFilePath);
    }

    public void ConvertAudioFormat()
    {
        var originalVideoPath = "Examples/Multimedia/744946__crackerjack56__downed.mp3";
        var outputFilePath = Path.Combine(_outputPath, $"ChangeFormat-{Guid.NewGuid()}");
        _audioTranscodingProcessor.ConvertAudioFormat(originalVideoPath, outputFilePath, "flac");
        _logger.LogInformation("Final audio located at: {Path}", outputFilePath);
    }
}
