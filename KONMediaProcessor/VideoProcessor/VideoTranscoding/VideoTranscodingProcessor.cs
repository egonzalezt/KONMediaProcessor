namespace KONMediaProcessor.VideoProcessor.VideoTranscoding;

using Config;
using Domain.Exceptions;
using FFmpegExecutor;
using FileValidator;
using Domain.Shared;
using VideoProcessor.VideoInfo;
using Domain.VideoInfo;

internal class VideoTranscodingProcessor(IFFmpegExecutor executor, IVideoInfoProcessor videoInfoProcessor, IFileValidator fileValidator) : IVideoTranscodingProcessor
{
    private readonly IFFmpegExecutor _executor = executor;
    private readonly IFileValidator _fileValidator = fileValidator;
    private readonly IVideoInfoProcessor _videoInfoProcessor = videoInfoProcessor;
    
    public void TranscodeVideo(string inputFilePath, string outputFilePath, VideoCodec videoEncoder = VideoCodec.H264, AudioCodec audioEncoder = AudioCodec.AAC, int audioBitrate = 128, bool overrideFile = false, CancellationToken cancellationToken = default)
    {
        var inputs = new string[] { inputFilePath };
        _fileValidator.ValidatePaths(inputs, outputFilePath, overrideFile);
        string arguments = $"-i \"{inputFilePath}\" -c:v {videoEncoder.ToString().ToLower()} -c:a {audioEncoder.ToString().ToLower()} -b:a {audioBitrate}k \"{outputFilePath}\"";
        arguments += overrideFile ? " -y" : " -n";
        string result = _executor.ExecuteCommand(FFmpegConfig.GetFFmpegLocation(), arguments, cancellationToken);

        if (!string.IsNullOrEmpty(result) && result.Contains("Error:"))
        {
            throw new FFmpegException($"Error transcoding video: {result}");
        }
    }

    public void ChangeVideoResolution(string inputFilePath, string outputFilePath, int newWidth, int newHeight, bool overrideFile = false, CancellationToken cancellationToken = default)
    {
        var inputs = new string[] { inputFilePath };
        _fileValidator.ValidatePaths(inputs, outputFilePath, overrideFile);
        var arguments = $"-i \"{inputFilePath}\" -vf scale={newWidth}:{newHeight} \"{outputFilePath}\"";
        arguments += overrideFile ? " -y" : " -n";
        var result = _executor.ExecuteCommand(FFmpegConfig.GetFFmpegLocation(), arguments, cancellationToken);

        if (!string.IsNullOrEmpty(result) && result.Contains("Error:"))
        {
            throw new FFmpegException($"Error changing video resolution: {result}");
        }
    }

    public void SetVideoFrameRate(string inputFilePath, string outputFilePath, int frameRate, bool overrideFile = false, CancellationToken cancellationToken = default)
    {
        if (frameRate <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(frameRate), "Frame rate must be greater than zero.");
        }

        var inputs = new string[] { inputFilePath };
        _fileValidator.ValidatePaths(inputs, outputFilePath, overrideFile);
        var arguments = $"-i \"{inputFilePath}\" -r {frameRate} \"{outputFilePath}\"";
        arguments += overrideFile ? " -y" : " -n";
        var result = _executor.ExecuteCommand(FFmpegConfig.GetFFmpegLocation(), arguments, cancellationToken);

        if (!string.IsNullOrEmpty(result) && result.Contains("Error:"))
        {
            throw new FFmpegException($"Error setting video frame rate: {result}");
        }
    }

    public void ConcatenateVideos(string[] inputFilePaths, string outputFilePath, bool includeAudio = true, bool overrideFile = false, CancellationToken cancellationToken = default)
    {
        if (inputFilePaths == null || inputFilePaths.Length == 0)
        {
            throw new ArgumentException("Input file paths cannot be null or empty.", nameof(inputFilePaths));
        }

        if (string.IsNullOrEmpty(outputFilePath))
        {
            throw new ArgumentNullException(nameof(outputFilePath));
        }

        _fileValidator.ValidatePaths(inputFilePaths, outputFilePath, overrideFile);

        var firstVideoInfo = _videoInfoProcessor.GetVideoInfo(inputFilePaths[0]);
        for (int i = 1; i < inputFilePaths.Length; i++)
        {
            var videoInfo = _videoInfoProcessor.GetVideoInfo(inputFilePaths[i]);
            if (videoInfo.Width != firstVideoInfo.Width || videoInfo.Height != firstVideoInfo.Height)
            {
                throw new DifferentResolutionsException("Videos have different resolutions. Concatenation is not supported.");
            }
        }

        string inputFiles = string.Join(" ", inputFilePaths.Select(path => $"-i \"{path}\""));
        var arguments = $" {inputFiles} -filter_complex \"";

        if (includeAudio)
        {
            for (int i = 0; i < inputFilePaths.Length; i++)
            {
                var videoInfo = _videoInfoProcessor.GetVideoInfo(inputFilePaths[i]);
                arguments += $"[{i}:v] [{i}:a]";
            }
            arguments += $" concat=n={inputFilePaths.Length}:v=1:a=1 [v] [a]\"";
        }
        else
        {
            for (int i = 0; i < inputFilePaths.Length; i++)
            {
                arguments += $"[{i}:v]";
            }
            arguments += $" concat=n={inputFilePaths.Length}:v=1 [v]\"";
        }

        arguments += $" -map \"[v]\"";

        if (includeAudio)
        {
            arguments += $" -map \"[a]\"";
        }

        arguments += $" \"{outputFilePath}\"";
        arguments += overrideFile ? " -y" : " -n";

        var result = _executor.ExecuteCommand(FFmpegConfig.GetFFmpegLocation(), arguments, cancellationToken);
        if (!string.IsNullOrEmpty(result) && result.Contains("Error:"))
        {
            throw new FFmpegException($"Error concatenating videos: {result}");
        }
    }

    public void ChangeAspectRatio(string inputFilePath, string outputFilePath, AspectRatio aspectRatio, bool overrideFile = false, CancellationToken cancellationToken = default)
    {
        var inputs = new string[] { inputFilePath };
        _fileValidator.ValidatePaths(inputs, outputFilePath, overrideFile);
        var arguments = $"-i \"{inputFilePath}\" -vf \"scale=iw:ih,setsar={aspectRatio}\" -c:a copy \"{outputFilePath}\"";
        arguments += overrideFile ? " -y" : " -n";
        var result = _executor.ExecuteCommand(FFmpegConfig.GetFFmpegLocation(), arguments, cancellationToken);
        if (!string.IsNullOrEmpty(result) && result.Contains("Error:"))
        {
            throw new FFmpegException($"Error concatenating videos: {result}");
        }
    }
}
