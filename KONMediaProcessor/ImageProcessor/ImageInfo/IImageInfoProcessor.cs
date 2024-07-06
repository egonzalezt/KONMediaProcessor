namespace KONMediaProcessor.ImageProcessor.ImageInfo;

using Domain.ImageInfo;


/// <summary>
/// Provides methods to retrieve information about images.
/// </summary>
public interface IImageInfoProcessor
{
    /// <summary>
    /// Retrieves information about the specified image file.
    /// </summary>
    /// <param name="inputFile">The path to the input image file.</param>
    /// <param name="cancellationToken">Cancellation token to stop the process</param>
    /// <returns>An <see cref="ImageInfo"/> object containing information about the image.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the specified input image file does not exist.</exception>
    /// <exception cref="FFmpegException">Thrown when an error occurs during the retrieval of image information using FFprobe.</exception>
    ImageInfo GetImageInfo(string inputFile, CancellationToken cancellationToken = default);
}