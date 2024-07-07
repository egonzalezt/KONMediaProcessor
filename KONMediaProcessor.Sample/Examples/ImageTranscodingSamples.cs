namespace KONMediaProcessor.Sample.Examples;

using Domain.ImageInfo;
using ImageProcessor.ImageTranscoding;
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
            new() { X = 0, Y = 0, Text = "Hello team!" },
            new() { X = 0, Y = 100, Text = "This is a test" },
            new() { X = 0, Y = 200, Text = "using facade" }
        };
        var font = "Examples/Fonts/Arial.ttf";
        var textColor = "white";
        var backgroundColor = "black";
        var width = 1000;
        var height = 300;
        var outputFilePath = Path.Combine(_outputPath, $"CreateImage-{Guid.NewGuid()}.jpg");
        var fontSize = 100;
        _imageTranscodingProcessor.GenerateImage(textDataList, font, textColor, backgroundColor, width, height, fontSize, outputFilePath);
        _logger.LogInformation("Final image located at: {Path}", outputFilePath);
    }

    public void JoinImages()
    {
        var images = new List<ImageData>
        {
            new() { X = 0, Y = 200, Path = "Examples/Multimedia/FFmpeg_Logo_new.png" },
            new() { X = 500, Y = 200, Path = "Examples/Multimedia/k-on.png" }
        };
        var backgroundColor = "black";
        var outputFilePath = Path.Combine(_outputPath, $"JoinImages-{Guid.NewGuid()}.png");
        _imageTranscodingProcessor.CombineImages(images, 1920, 1080, outputFilePath, backgroundColor);
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
}
