namespace KONMediaProcessor.Sample.Examples;

using VideoProcessor.VideoTranscoding;
using Microsoft.Extensions.Logging;
using KONMediaProcessor.Domain.VideoInfo;

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
        var outputFilePath = Path.Combine(_outputPath, $"Transcode-{Guid.NewGuid()}.mp4");
        _videoTranscodingProcessor.TranscodeVideo(originalVideoPath, outputFilePath, Domain.Shared.VideoCodec.MPEG4, Domain.Shared.AudioCodec.FLAC);
        _logger.LogInformation("Final video located at: {Path}", outputFilePath);
    }

    public void ChangeVideoResolution()
    {
        var originalVideoPath = "Examples/Multimedia/sampleVideo2.mp4";
        var outputFilePath = Path.Combine(_outputPath, $"ChangeRes-{Guid.NewGuid()}.mp4");
        _videoTranscodingProcessor.ChangeVideoResolution(originalVideoPath, outputFilePath, 3840, 2160);
    }

    public void SetVideoFrameRate()
    {
        var originalVideoPath = "Examples/Multimedia/sampleVideo2.mp4";
        var outputFilePath = Path.Combine(_outputPath, $"ChangeFPS-{Guid.NewGuid()}.mp4");
        _videoTranscodingProcessor.SetVideoFrameRate(originalVideoPath, outputFilePath, 60);
    }

    public void ConcatenateVideos()
    {
        var outputFilePath = Path.Combine(_outputPath, $"JoinVideos-{Guid.NewGuid()}.mp4");
        var paths = new List<string>()
        {
            "Examples/Multimedia/sampleVideo.mp4",
            "Examples/Multimedia/sampleVideo2.mp4"
        };
        _videoTranscodingProcessor.ConcatenateVideos([.. paths], outputFilePath);
    }

    public void ChangeAspectRatio()
    {
        var originalVideoPath = "Examples/Multimedia/sampleVideo2.mp4";
        var outputFilePath = Path.Combine(_outputPath, $"AspectRatio-{Guid.NewGuid()}.mp4");
        var aspectRatio = new AspectRatio(1920,980);
        _videoTranscodingProcessor.ChangeAspectRatio(originalVideoPath, outputFilePath, aspectRatio);
    }
}
