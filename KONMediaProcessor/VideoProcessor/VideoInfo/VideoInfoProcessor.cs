namespace KONMediaProcessor.VideoProcessor.VideoInfo;

using Entities;
using Entities.Dtos;
using Exceptions;
using Shared;
using FFmpegExecutor;
using System.Linq;
using System.Text.Json;
using FileValidator;

internal class VideoInfoProcessor(IFFmpegExecutor executor, IFileValidator fileValidator) : IVideoInfoProcessor
{
    private readonly IFFmpegExecutor _executor = executor;
    private readonly IFileValidator _fileValidator = fileValidator;

    public VideoInfo GetVideoInfo(string inputFile)
    {
        var processedInputFile = _fileValidator.ValidateFileExists(inputFile);
        string arguments = $"-v error -select_streams v:0 -show_entries stream=width,height,avg_frame_rate -of json \"{processedInputFile}\"";
        string jsonResult = _executor.ExecuteCommand(SupportedExecutors.ffprobe, arguments) ?? throw new FFmpegException("FFmpeg does not return any result"); ;
        var ffProbeResult = JsonSerializer.Deserialize<FFprobeResultDto>(jsonResult) ?? throw new FFmpegException("FFmpeg does not return any result"); ;
        var streamInfo = ffProbeResult.Streams.FirstOrDefault() ?? throw new FFmpegException("No video data was found in the file provided.");

        return new VideoInfo
        {
            Width = streamInfo.Width,
            Height = streamInfo.Height,
            FrameRate = CalculateFrameRate(streamInfo.AvgFrameRate)
        };
    }

    private static double CalculateFrameRate(string avgFrameRate)
    {
        if (!string.IsNullOrEmpty(avgFrameRate) && avgFrameRate.Contains("/"))
        {
            string[] parts = avgFrameRate.Split('/');
            if (parts.Length == 2 && int.TryParse(parts[0], out int numerator) && int.TryParse(parts[1], out int denominator) && denominator != 0)
            {
                return (double)numerator / denominator;
            }
        }
        return 0;
    }
}

