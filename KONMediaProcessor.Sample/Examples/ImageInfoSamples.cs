namespace KONMediaProcessor.Sample.Examples.Image;

using ImageProcessor.ImageInfo;
using Microsoft.Extensions.Logging;

public class ImageInfoSamples(IImageInfoProcessor imageInfo, ILogger<ImageInfoSamples> logger)
{
    const string sampleImagePath = "Examples/Multimedia/k-on.png";
    public void GetImageInformation()
    {
        var imageInformation = imageInfo.GetImageInfo(sampleImagePath);
        logger.LogInformation("With: {W}, Height: {H}, Format: {F}", imageInformation.Width, imageInformation.Height, imageInformation.Format);
    }
}
