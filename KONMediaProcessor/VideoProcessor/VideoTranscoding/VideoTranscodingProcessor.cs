namespace KONMediaProcessor.VideoProcessor.VideoTranscoding;

using Exceptions;
using FFmpegExecutor;
using FileValidator;
using Shared;
using VideoProcessor.VideoInfo;
using VideoInfo.Entities;

internal class VideoTranscodingProcessor(IFFmpegExecutor executor, IVideoInfoProcessor videoInfoProcessor, IFileValidator fileValidator) : IVideoTranscodingProcessor
{
    private readonly IFFmpegExecutor _executor = executor;
    private readonly IFileValidator _fileValidator = fileValidator;
    private readonly IVideoInfoProcessor _videoInfoProcessor = videoInfoProcessor;
    
    public void TranscodeVideo(string inputFilePath, string outputFilePath, VideoCodec videoEncoder = VideoCodec.H264, AudioCodec audioEncoder = AudioCodec.AAC, int audioBitrate = 128, bool overrideFile = false)
    {
        var (validatedInputs, validatedOutput) = _fileValidator.ValidatePaths([inputFilePath], outputFilePath, overrideFile);
        string arguments = $"-i \"{validatedInputs.First()}\" -c:v {videoEncoder.ToString().ToLower()} -c:a {audioEncoder.ToString().ToLower()} -b:a {audioBitrate}k \"{validatedOutput}\"";
        arguments += overrideFile ? " -y" : " -n";
        string result = _executor.ExecuteCommand(SupportedExecutors.ffmpeg, arguments);

        if (!string.IsNullOrEmpty(result) && result.Contains("Error:"))
        {
            throw new FFmpegException($"Error transcoding video: {result}");
        }
    }

    public void ChangeVideoResolution(string inputFilePath, string outputFilePath, int newWidth, int newHeight, bool overrideFile = false)
    {
        var (validatedInputs, validatedOutput) = _fileValidator.ValidatePaths([inputFilePath], outputFilePath, overrideFile);
        var arguments = $"-i \"{validatedInputs.First()}\" -vf scale={newWidth}:{newHeight} \"{validatedOutput}\"";
        arguments += overrideFile ? " -y" : " -n";
        var result = _executor.ExecuteCommand(SupportedExecutors.ffmpeg, arguments);

        if (!string.IsNullOrEmpty(result) && result.Contains("Error:"))
        {
            throw new FFmpegException($"Error changing video resolution: {result}");
        }
    }

    public void SetVideoFrameRate(string inputFilePath, string outputFilePath, int frameRate, bool overrideFile = false)
    {
        if (frameRate <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(frameRate), "Frame rate must be greater than zero.");
        }

        var (validatedInputs, validatedOutput) = _fileValidator.ValidatePaths([inputFilePath], outputFilePath, overrideFile);
        var arguments = $"-i \"{validatedInputs.First()}\" -r {frameRate} \"{validatedOutput}\"";
        arguments += overrideFile ? " -y" : " -n";
        var result = _executor.ExecuteCommand(SupportedExecutors.ffmpeg, arguments);

        if (!string.IsNullOrEmpty(result) && result.Contains("Error:"))
        {
            throw new FFmpegException($"Error setting video frame rate: {result}");
        }
    }

    public void ConcatenateVideos(string[] inputFilePaths, string outputFilePath, bool includeAudio = true, bool overrideFile = false)
    {
        var (validatedInputs, validatedOutput) = _fileValidator.ValidatePaths(inputFilePaths, outputFilePath, overrideFile);

        var firstVideoInfo = _videoInfoProcessor.GetVideoInfo(validatedInputs[0]);
        for (int i = 1; i < validatedInputs.Length; i++)
        {
            var videoInfo = _videoInfoProcessor.GetVideoInfo(validatedInputs[i]);
            if (videoInfo.Width != firstVideoInfo.Width || videoInfo.Height != firstVideoInfo.Height)
            {
                throw new DifferentResolutionsException("Videos have different resolutions. Concatenation is not supported.");
            }
        }

        string inputFiles = string.Join(" ", validatedInputs.Select(path => $"-i \"{path}\""));
        var arguments = $" {inputFiles} -filter_complex \"";

        if (includeAudio)
        {
            for (int i = 0; i < validatedInputs.Length; i++)
            {
                var videoInfo = _videoInfoProcessor.GetVideoInfo(validatedInputs[i]);
                arguments += $"[{i}:v] [{i}:a]";
            }
            arguments += $" concat=n={validatedInputs.Length}:v=1:a=1 [v] [a]\"";
        }
        else
        {
            for (int i = 0; i < validatedInputs.Length; i++)
            {
                arguments += $"[{i}:v]";
            }
            arguments += $" concat=n={validatedInputs.Length}:v=1 [v]\"";
        }

        arguments += $" -map \"[v]\"";

        if (includeAudio)
        {
            arguments += $" -map \"[a]\"";
        }

        arguments += $" \"{validatedOutput}\"";
        arguments += overrideFile ? " -y" : " -n";

        var result = _executor.ExecuteCommand(SupportedExecutors.ffmpeg, arguments);
        if (!string.IsNullOrEmpty(result) && result.Contains("Error:"))
        {
            throw new FFmpegException($"Error concatenating videos: {result}");
        }
    }

    public void ChangeAspectRatio(string inputFilePath, string outputFilePath, AspectRatio aspectRatio, bool overrideFile = false)
    {
        var (validatedInputs, validatedOutput) = _fileValidator.ValidatePaths([inputFilePath], outputFilePath, overrideFile);
        var arguments = $"-i \"{validatedInputs.First()}\" -vf \"scale=iw:ih,setsar={aspectRatio}\" -c:a copy \"{validatedOutput}\"";
        arguments += overrideFile ? " -y" : " -n";
        var result = _executor.ExecuteCommand(SupportedExecutors.ffmpeg, arguments);
        if (!string.IsNullOrEmpty(result) && result.Contains("Error:"))
        {
            throw new FFmpegException($"Error concatenating videos: {result}");
        }
    }
}
