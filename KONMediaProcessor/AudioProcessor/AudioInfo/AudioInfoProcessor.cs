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

    public AudioInfo GetAudioInfo(string inputFile, CancellationToken cancellationToken = default)
    {
        if (!_fileValidator.FileExists(inputFile))
        {
            throw new FileNotFoundException(inputFile);
        }

        string arguments = $"-v error -select_streams a:0 -show_entries stream=codec_name,sample_rate,channels -of json \"{inputFile}\"";
        string jsonResult = _executor.ExecuteCommand(SupportedExecutors.ffprobe, arguments, cancellationToken);

        var ffprobeResult = JsonSerializer.Deserialize<FFprobeAudioResultDto>(jsonResult);

        if (ffprobeResult == null || !ffprobeResult.Streams.Any())
        {
            throw new FFmpegException("No audio data was found in the file provided.");
        }

        var streamInfo = ffprobeResult.Streams.First();

        return new AudioInfo
        {
            Codec = streamInfo.CodecName,
            SampleRate = streamInfo.SampleRate,
            Channels = streamInfo.Channels
        };
    }
}