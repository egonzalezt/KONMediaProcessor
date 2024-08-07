﻿namespace KONMediaProcessor.Sample.Examples;

using ImageProcessor.ImageTranscoding;
using ImageProcessor.ImageInfo.Entities;
using Microsoft.Extensions.Logging;

public class ImageTranscodingSamples
{
    private readonly string _outputPath;
    private readonly IImageTranscodingProcessor _imageTranscodingProcessor;
    private readonly ILogger _logger;

    public ImageTranscodingSamples(IImageTranscodingProcessor imageTranscodingProcessor, ILogger<ImageTranscodingSamples> logger)
    {
        _imageTranscodingProcessor = imageTranscodingProcessor;
        _logger = logger;
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string className = GetType().Name;
        _outputPath = Path.Combine(desktopPath, nameof(KONMediaProcessor) ,className);

        if (!Directory.Exists(_outputPath))
        {
            Directory.CreateDirectory(_outputPath);
        }
    }

    public void CreateImage()
    {
        var textDataList = new List<TextData>
        {
            new(0, 0, @"""Hello team""\", "#12D7CE", 50),
            new(0, 100, "This is a test", "#49DE15", 50),
            new(0, 200, "using facade test", "#C118D5", 50)
        };
        var font = "Examples/Fonts/cour.ttf";
        var backgroundColor = "black";
        var width = 1000;
        var height = 300;
        var outputFilePath = Path.Combine(_outputPath, $"CreateImage-{Guid.NewGuid()}.jpg");
        var canvas = new Canvas(width, height, backgroundColor, outputFilePath);
        _imageTranscodingProcessor.GenerateImage(textDataList, canvas, font);
        _logger.LogInformation("Final image located at: {Path}", outputFilePath);
    }

    public void JoinImages()
    {
        var images = new List<ImageData>
        {
            new("Examples/Multimedia/FFmpeg_Logo_new.png", 0, 200),
            new("Examples/Multimedia/k-on.png", 500, 200)
        };
        var backgroundColor = "black";
        var outputFilePath = Path.Combine(_outputPath, $"JoinImages-{Guid.NewGuid()}.png");
        var canvas = new Canvas(1920, 1080, backgroundColor, outputFilePath);
        _imageTranscodingProcessor.CombineImages(images, canvas);
        _logger.LogInformation("Final image located at: {Path}", outputFilePath);
    }

    public void ResizeImage()
    {
        var outputFilePath = Path.Combine(_outputPath, $"Resized-{Guid.NewGuid()}.png");
        _imageTranscodingProcessor.ResizeImage("Examples/Multimedia/k-on.png", 3840, 2160, outputFilePath);
        _logger.LogInformation("Resized image located at: {Path}", outputFilePath);
    }

    public void ImageToVideo()
    {
        var outputFilePath = Path.Combine(_outputPath, $"ImgToVideo-{Guid.NewGuid()}.mp4");
        _imageTranscodingProcessor.ConvertImageToVideo("Examples/Multimedia/k-on.png", 10, outputFilePath);
        _logger.LogInformation("Video created from image located at: {Path}", outputFilePath);

        var outputFilePath2 = Path.Combine(_outputPath, $"ImgToVideo-{Guid.NewGuid()}.mp4");
        _imageTranscodingProcessor.ConvertImageToVideo("Examples/Multimedia/k-on.png", 2, 1920, 1080, outputFilePath2);
        _logger.LogInformation("Video created from image located at: {Path}", outputFilePath2);
    }

    public void ImageAsBase64()
    {
        _imageTranscodingProcessor.ImageToBase64("Examples/Multimedia/k-on.png");
    }
}
