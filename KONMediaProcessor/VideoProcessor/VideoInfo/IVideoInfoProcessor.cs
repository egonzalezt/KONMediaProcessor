namespace KONMediaProcessor.VideoProcessor.VideoInfo;

using Entities;

/// <summary>
/// Provides methods to retrieve information about videos.
/// </summary>
public interface IVideoInfoProcessor
{
    /// <summary>
    /// Retrieves information about the specified video file.
    /// </summary>
    /// <param name="inputFile">The path to the input video file.</param>
    /// <param name="cancellationToken">Cancellation token to stop the process</param>
    /// <returns>A <see cref="VideoInfo"/> object containing information about the video.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the specified input video file does not exist.</exception>
    /// <exception cref="FFmpegException">Thrown when an error occurs during the retrieval of video information using FFprobe.</exception>
    VideoInfo GetVideoInfo(string inputFile, CancellationToken cancellationToken = default);
}