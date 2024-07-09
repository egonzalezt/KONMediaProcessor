namespace KONMediaProcessor.VideoProcessor.VideoTranscoding;

using Shared;
using VideoInfo.Entities;
using Exceptions;

/// <summary>
/// Provides methods for transcoding, modifying, and concatenating video files.
/// </summary>
public interface IVideoTranscodingProcessor
{
    /// <summary>
    /// Transcodes a video file to a different format or codec.
    /// </summary>
    /// <param name="inputFilePath">The path to the input video file.</param>
    /// <param name="outputFilePath">The path to save the transcoded video.</param>
    /// <param name="videoEncoder">The video codec to use for transcoding (default is H.264).</param>
    /// <param name="audioEncoder">The audio codec to use for transcoding (default is AAC).</param>
    /// <param name="audioBitrate">The audio bitrate in Kbps (default is 128 Kbps).</param>
    /// <param name="overrideFile">True to override the output file if it already exists, false otherwise (default is false).</param>
    void TranscodeVideo(string inputFilePath, string outputFilePath, VideoCodec videoEncoder = VideoCodec.H264, AudioCodec audioEncoder = AudioCodec.AAC, int audioBitrate = 128, bool overrideFile = false);

    /// <summary>
    /// Changes the resolution of a video file.
    /// </summary>
    /// <param name="inputFilePath">The path to the input video file.</param>
    /// <param name="outputFilePath">The path to save the video with the new resolution.</param>
    /// <param name="newWidth">The new width of the video.</param>
    /// <param name="newHeight">The new height of the video.</param>
    /// <param name="overrideFile">True to override the output file if it already exists, false otherwise (default is false).</param>
    void ChangeVideoResolution(string inputFilePath, string outputFilePath, int newWidth, int newHeight, bool overrideFile = false);

    /// <summary>
    /// Sets the frame rate of a video file.
    /// </summary>
    /// <param name="inputFilePath">The path to the input video file.</param>
    /// <param name="outputFilePath">The path to save the video with the new frame rate.</param>
    /// <param name="frameRate">The new frame rate of the video in frames per second (FPS).</param>
    /// <param name="overrideFile">True to override the output file if it already exists, false otherwise (default is false).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the frame rate is less than or equal to zero.</exception>
    void SetVideoFrameRate(string inputFilePath, string outputFilePath, int frameRate, bool overrideFile = false);

    /// <summary>
    /// Concatenates multiple video files into a single video file.
    /// </summary>
    /// <param name="inputFilePaths">The paths to the input video files to concatenate.</param>
    /// <param name="outputFilePath">The path to save the concatenated video.</param>
    /// <param name="includeAudio">True to include audio from input files, false otherwise (default is true).</param>
    /// <param name="overrideFile">True to override the output file if it already exists, false otherwise (default is false).</param>
    /// <exception cref="ArgumentException">Thrown when the input file paths array is null or empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the output file path is null or empty.</exception>
    /// <exception cref="DifferentResolutionsException">Thrown when the input videos have different resolutions, and concatenation is not supported.</exception>
    void ConcatenateVideos(string[] inputFilePaths, string outputFilePath, bool includeAudio = true, bool overrideFile = false);

    /// <summary>
    /// Changes the aspect ratio of a video file.
    /// </summary>
    /// <param name="inputFilePath">The path to the input video file.</param>
    /// <param name="outputFilePath">The path to save the video with the new aspect ratio.</param>
    /// <param name="aspectRatio">The new aspect ratio to set for the video.</param>
    /// <param name="overrideFile">True to override the output file if it already exists, false otherwise (default is false).</param>
    void ChangeAspectRatio(string inputFilePath, string outputFilePath, AspectRatio aspectRatio, bool overrideFile = false);
}