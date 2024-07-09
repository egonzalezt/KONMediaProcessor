namespace KONMediaProcessor.AudioProcessor.AudioInfo;

using Exceptions;
using FFmpegExecutor;
using FileValidator;
using System.Text.Json;
using Shared;
using Entities;
using Entities.Dtos;

internal class AudioInfoProcessor(IFFmpegExecutor executor, IFileValidator fileValidator) : IAudioInfoProcessor
{
    private readonly IFFmpegExecutor _executor = executor;
    private readonly IFileValidator _fileValidator = fileValidator;

    public AudioInfo GetAudioInfo(string inputFile)
    {
        var processedInputFile = _fileValidator.ValidateFileExists(inputFile);
        string arguments = $"-v error -select_streams a:0 -show_entries stream=codec_name,sample_rate,channels -of json \"{processedInputFile}\"";
        string jsonResult = _executor.ExecuteCommand(SupportedExecutors.ffprobe, arguments) ?? throw new FFmpegException("FFmpeg does not return any result");
        var ffProbeResult = JsonSerializer.Deserialize<FFprobeAudioResultDto>(jsonResult);

        if (ffProbeResult == null || ffProbeResult.Streams.Count == 0)
        {
            throw new FFmpegException("No audio data was found in the file provided.");
        }

        var streamInfo = ffProbeResult.Streams.First();

        return new AudioInfo
        {
            Codec = streamInfo.CodecName,
            SampleRate = streamInfo.SampleRate,
            Channels = streamInfo.Channels
        };
    }
}