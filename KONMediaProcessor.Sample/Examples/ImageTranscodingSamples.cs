namespace KONMediaProcessor.Sample.Examples;

using Domain.ImageInfo;
using ImageProcessor.ImageTranscoding;
using Microsoft.Extensions.Logging;

public class ImageTranscodingSamples(IImageTranscodingProcessor imageTranscodingProcessor, ILogger<ImageTranscodingSamples> logger)
{
    public void CreateImage()
    {
        var textDataList = new List<TextData>
        {
            new TextData { X = 0, Y = 0, Text = "Hello, world!" },
            new TextData { X = 0, Y = 100, Text = "This is a test" }
        };
        var font = "Examples/Fonts/Arial.ttf";
        var textColor = "white";
        var backgroundColor = "black";
        var width = 1000;
        var height = 300;
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var outputFilePath = Path.Join(path, $"{Guid.NewGuid()}.jpg");
        var fontSize = 100;
        imageTranscodingProcessor.GenerateImage(textDataList, font, textColor, backgroundColor, width, height, fontSize, outputFilePath);
        logger.LogInformation("Final image located at: {Path}", outputFilePath);
    }

    public void JoinImages()
    {
        var images = new List<ImageData> {
            new() { X = 0, Y = 200, Path = "Examples/Multimedia/FFmpeg_Logo_new.png" },
            new() { X = 500, Y = 200, Path = "Examples/Multimedia/k-on.png" },
        };
        var backgroundColor = "black";
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var outputFilePath = Path.Join(path, $"{Guid.NewGuid()}.png");
        imageTranscodingProcessor.CombineImages(images, 1920, 1080, outputFilePath, backgroundColor);
        logger.LogInformation("Final image located at: {Path}", outputFilePath);
    }

    public void ResizeImage()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var outputFilePath = Path.Join(path, $"{Guid.NewGuid()}.png");
        imageTranscodingProcessor.ResizeImage("Examples/Multimedia/k-on.png", 3840, 2160, outputFilePath);
    }

    public void ImageToVideo()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var outputFilePath = Path.Join(path, $"{Guid.NewGuid()}.mp4");
        imageTranscodingProcessor.ConvertImageToVideo("Examples/Multimedia/k-on.png", 10, outputFilePath);
        var outputFilePath2 = Path.Join(path, $"{Guid.NewGuid()}.mp4");
        imageTranscodingProcessor.ConvertImageToVideo("Examples/Multimedia/k-on.png",2, 1920, 1080, outputFilePath2);
    }
}
