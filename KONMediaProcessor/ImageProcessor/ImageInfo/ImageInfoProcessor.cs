﻿namespace KONMediaProcessor.ImageProcessor.ImageInfo;

using Config;
using Domain.Exceptions;
using Domain.ImageInfo.Dtos;
using Domain.ImageInfo;
using FFmpegExecutor;
using FileValidator;
using System.Text.Json;

public class ImageInfoProcessor(IFFmpegExecutor executor, IFileValidator fileValidator) : IImageInfoProcessor
{
    private readonly IFFmpegExecutor _executor = executor;
    private readonly IFileValidator _fileValidator = fileValidator;

    public ImageInfo GetImageInfo(string inputFile)
    {
        if (!_fileValidator.FileExists(inputFile))
        {
            throw new FileNotFoundException(inputFile);
        }

        string arguments = $"-v error -select_streams v:0 -show_entries stream=width,height,pix_fmt -of json \"{inputFile}\"";
        string jsonResult = _executor.ExecuteCommand(FFmpegConfig.GetFFprobeLocation(), arguments);

        var ffprobeResult = JsonSerializer.Deserialize<FFprobeImageResultDto>(jsonResult) ?? throw new FFmpegException("FFmpeg does not return any result");
        var streamInfo = ffprobeResult.Streams.FirstOrDefault() ?? throw new FFmpegException("No image data was found in the file provided.");

        return new ImageInfo
        {
            Width = streamInfo.Width,
            Height = streamInfo.Height,
            Format = streamInfo.PixFmt
        };
    }
}
