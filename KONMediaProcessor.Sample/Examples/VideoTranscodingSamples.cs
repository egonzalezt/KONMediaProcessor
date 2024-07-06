namespace KONMediaProcessor.Sample.Examples;

using VideoProcessor.VideoTranscoding;
using Microsoft.Extensions.Logging;

public class VideoTranscodingSamples
{
    private readonly string _outputPath;
    private readonly IVideoTranscodingProcessor _videoTranscodingProcessor;
    private readonly ILogger<VideoTranscodingSamples> _logger;

    public VideoTranscodingSamples(IVideoTranscodingProcessor videoTranscodingProcessor, ILogger<VideoTranscodingSamples> logger)
    {
        _videoTranscodingProcessor = videoTranscodingProcessor;
        _logger = logger;
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string className = GetType().Name;
        _outputPath = Path.Combine(desktopPath, nameof(KONMediaProcessor), className);

        if (!Directory.Exists(_outputPath))
        {
            Directory.CreateDirectory(_outputPath);
        }
    }

    public void TranscodeVideo()
    {
        var originalVideoPath = "Examples/Multimedia/sampleVideo.mp4";
        var outputFilePath = Path.Combine(_outputPath, $"{Guid.NewGuid()}.mp4");
        _videoTranscodingProcessor.TranscodeVideo(originalVideoPath, outputFilePath, Domain.Shared.VideoCodec.MPEG4, Domain.Shared.AudioCodec.FLAC);
        _logger.LogInformation("Final video located at: {Path}", outputFilePath);
    }

    public void ChangeVideoResolution()
    {
        var originalVideoPath = "Examples/Multimedia/sampleVideo2.mp4";
        var outputFilePath = Path.Combine(_outputPath, $"{Guid.NewGuid()}.mp4");
        _videoTranscodingProcessor.ChangeVideoResolution(originalVideoPath, outputFilePath, 3840, 2160);
    }
}
