namespace KONMediaProcessor.AudioProcessor.AudioTranscoding;

using Shared;
using KONMediaProcessor.Exceptions;
using Exceptions;
/// <summary>
/// Interface for audio transcoding operations.
/// </summary>
public interface IAudioTranscodingProcessor
{
    /// <summary>
    /// Transcodes an audio file to a different format, codec, and bitrate.
    /// </summary>
    /// <param name="inputFilePath">The path to the input audio file.</param>
    /// <param name="outputFilePath">The path to the output audio file.</param>
    /// <param name="audioEncoder">The audio codec to use for transcoding (default is AAC).</param>
    /// <param name="audioBitrate">The audio bitrate for the output file (default is 128 kbps).</param>
    /// <param name="overrideFile">If true, override the output file if it exists (default is false).</param>
    /// <returns>The path where the new file is located.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the input file does not exist.</exception>
    /// <exception cref="FFmpegException">Thrown when an error occurs during transcoding.</exception>
    string TranscodeAudio(string inputFilePath, string outputFilePath, AudioCodec audioEncoder = AudioCodec.AAC, int audioBitrate = 128, bool overrideFile = false);

    /// <summary>
    /// Changes the bitrate of an audio file.
    /// </summary>
    /// <param name="inputFilePath">The path to the input audio file.</param>
    /// <param name="outputFilePath">The path to the output audio file.</param>
    /// <param name="newBitrate">The new bitrate for the output file.</param>
    /// <param name="overrideFile">If true, override the output file if it exists (default is false).</param>
    /// <exception cref="FileNotFoundException">Thrown when the input file does not exist.</exception>
    /// <exception cref="FFmpegException">Thrown when an error occurs during the bitrate change.</exception>
    void ChangeAudioBitrate(string inputFilePath, string outputFilePath, int newBitrate, bool overrideFile = false);

    /// <summary>
    /// Changes the number of audio channels in an audio file.
    /// </summary>
    /// <param name="inputFilePath">The path to the input audio file.</param>
    /// <param name="outputFilePath">The path to the output audio file.</param>
    /// <param name="channels">The number of audio channels for the output file.</param>
    /// <param name="overrideFile">If true, override the output file if it exists (default is false).</param>
    /// <exception cref="FileNotFoundException">Thrown when the input file does not exist.</exception>
    /// <exception cref="FFmpegException">Thrown when an error occurs during the channel change.</exception>
    void ChangeAudioChannels(string inputFilePath, string outputFilePath, int channels, bool overrideFile = false);

    /// <summary>
    /// Concatenates multiple audio files into a single audio file.
    /// </summary>
    /// <param name="inputFilePaths">An array of paths to the input audio files.</param>
    /// <param name="outputFilePath">The path to the output audio file.</param>
    /// <param name="overrideFile">If true, override the output file if it exists (default is false).</param>
    /// <exception cref="ArgumentException">Thrown when the input file paths array is null or empty.</exception>
    /// <exception cref="FileNotFoundException">Thrown when any of the input files do not exist.</exception>
    /// <exception cref="DifferentAudioPropertiesException">Thrown when the audio files have different properties (e.g., sample rate, channels).</exception>
    /// <exception cref="FFmpegException">Thrown when an error occurs during concatenation.</exception>
    void ConcatenateAudios(string[] inputFilePaths, string outputFilePath, bool overrideFile = false);

    /// <summary>
    /// Converts the format of an audio file.
    /// </summary>
    /// <param name="inputFilePath">The path to the input audio file.</param>
    /// <param name="outputFilePath">The path to the output audio file.</param>
    /// <param name="format">The format for the output file (e.g., "mp3", "wav").</param>
    /// <param name="overrideFile">If true, override the output file if it exists (default is false).</param>
    /// <exception cref="FileNotFoundException">Thrown when the input file does not exist.</exception>
    /// <exception cref="FFmpegException">Thrown when an error occurs during format conversion.</exception>
    void ConvertAudioFormat(string inputFilePath, string outputFilePath, string format, bool overrideFile = false);
}
