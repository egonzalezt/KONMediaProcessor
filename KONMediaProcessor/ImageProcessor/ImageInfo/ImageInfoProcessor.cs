namespace KONMediaProcessor.ImageProcessor.ImageInfo;

using Exceptions;
using Entities;
using Entities.Dtos;
using Shared;
using FFmpegExecutor;
using FileValidator;
using System.Text.Json;

internal class ImageInfoProcessor(IFFmpegExecutor executor, IFileValidator fileValidator) : IImageInfoProcessor
{
    private readonly IFFmpegExecutor _executor = executor;
    private readonly IFileValidator _fileValidator = fileValidator;

    public ImageInfo GetImageInfo(string inputFile)
    {
        var processedInputFile = _fileValidator.ValidateFileExists(inputFile);
        string arguments = $"-v error -select_streams v:0 -show_entries stream=width,height,pix_fmt -of json \"{processedInputFile}\"";
        string jsonResult = _executor.ExecuteCommand(SupportedExecutors.ffprobe, arguments) ?? throw new FFmpegException("FFmpeg does not return any result");
        var ffProbeResult = JsonSerializer.Deserialize<FFprobeImageResultDto>(jsonResult) ?? throw new FFmpegException("FFmpeg does not return any result");
        var streamInfo = ffProbeResult.Streams.FirstOrDefault() ?? throw new FFmpegException("No image data was found in the file provided.");

        return new ImageInfo
        {
            Width = streamInfo.Width,
            Height = streamInfo.Height,
            Format = streamInfo.PixFmt
        };
    }
}
