namespace KONMediaProcessor.AudioProcessor.AudioTranscoding;

using AudioProcessor.AudioInfo;
using Exceptions;
using Shared;
using FFmpegExecutor;
using FileValidator;
using KONMediaProcessor.Exceptions;

internal class AudioTranscodingProcessor(IFFmpegExecutor executor, IAudioInfoProcessor audioInfoProcessor, IFileValidator fileValidator) : IAudioTranscodingProcessor
{
    private readonly IFFmpegExecutor _executor = executor;
    private readonly IFileValidator _fileValidator = fileValidator;
    private readonly IAudioInfoProcessor _audioInfoProcessor = audioInfoProcessor;

    public string TranscodeAudio(string inputFilePath, string outputFilePath, AudioCodec audioEncoder = AudioCodec.AAC, int audioBitrate = 128, bool overrideFile = false)
    {
        var (validatedInputs, validatedOutput) = _fileValidator.ValidatePaths([inputFilePath], outputFilePath, overrideFile);
        string desiredExtension = GetFileExtensionForCodec(audioEncoder);
        outputFilePath = AdjustOutputFilePath(validatedOutput, desiredExtension);
        outputFilePath = AddExtensionIfMissing(outputFilePath, desiredExtension);

        string arguments = $"-i \"{validatedInputs.First()}\" -c:a {audioEncoder.ToString().ToLower()} -b:a {audioBitrate}k \"{outputFilePath}\"";
        arguments += overrideFile ? " -y" : " -n";

        string result = _executor.ExecuteCommand(SupportedExecutors.ffmpeg, arguments);

        if (!string.IsNullOrEmpty(result) && result.Contains("Error:"))
        {
            throw new FFmpegException($"Error transcoding audio: {result}");
        }

        return outputFilePath;
    }

    public void ChangeAudioBitrate(string inputFilePath, string outputFilePath, int newBitrate, bool overrideFile = false)
    {
        var (validatedInputs, validatedOutput) = _fileValidator.ValidatePaths([inputFilePath], outputFilePath, overrideFile);
        string arguments = $"-i \"{validatedInputs.First()}\" -b:a {newBitrate}k \"{validatedOutput}\"";
        arguments += overrideFile ? " -y" : " -n";
        string result = _executor.ExecuteCommand(SupportedExecutors.ffmpeg, arguments);

        if (!string.IsNullOrEmpty(result) && result.Contains("Error:"))
        {
            throw new FFmpegException($"Error changing audio bitrate: {result}");
        }
    }

    public void ChangeAudioChannels(string inputFilePath, string outputFilePath, int channels, bool overrideFile = false)
    {
        var (validatedInputs, validatedOutput) = _fileValidator.ValidatePaths([inputFilePath], outputFilePath, overrideFile);
        string arguments = $"-i \"{inputFilePath}\" -ac {channels} \"{outputFilePath}\"";
        arguments += overrideFile ? " -y" : " -n";
        string result = _executor.ExecuteCommand(SupportedExecutors.ffmpeg, arguments);

        if (!string.IsNullOrEmpty(result) && result.Contains("Error:"))
        {
            throw new FFmpegException($"Error changing audio channels: {result}");
        }
    }

    public void ConcatenateAudios(string[] inputFilePaths, string outputFilePath, bool overrideFile = false)
    {
        var (validatedInputs, validatedOutput) = _fileValidator.ValidatePaths(inputFilePaths, outputFilePath, overrideFile);

        var firstAudioInfo = _audioInfoProcessor.GetAudioInfo(validatedInputs[0]);
        for (int i = 1; i < validatedInputs.Length; i++)
        {
            var audioInfo = _audioInfoProcessor.GetAudioInfo(validatedInputs[i]);
            if (audioInfo.Channels != firstAudioInfo.Channels || audioInfo.SampleRate != firstAudioInfo.SampleRate)
            {
                throw new DifferentAudioPropertiesException("Audios have different properties. Concatenation is not supported.");
            }
        }

        string inputFiles = string.Join(" ", validatedInputs.Select(path => $"-i \"{path}\""));
        var arguments = $" {inputFiles} -filter_complex \"";

        for (int i = 0; i < validatedInputs.Length; i++)
        {
            arguments += $"[{i}:a]";
        }

        arguments += $" concat=n={validatedInputs.Length}:v=0:a=1 [a]\" -map \"[a]\" \"{validatedOutput}\"";
        arguments += overrideFile ? " -y" : " -n";

        var result = _executor.ExecuteCommand(SupportedExecutors.ffmpeg, arguments);
        if (!string.IsNullOrEmpty(result) && result.Contains("Error:"))
        {
            throw new FFmpegException($"Error concatenating audios: {result}");
        }
    }

    public void ConvertAudioFormat(string inputFilePath, string outputFilePath, string format, bool overrideFile = false)
    {
        var (validatedInputs, validatedOutput) = _fileValidator.ValidatePaths([inputFilePath], outputFilePath, overrideFile);
        var arguments = $"-i \"{validatedInputs.First()}\" \"{validatedOutput}.{format}\"";
        arguments += overrideFile ? " -y" : " -n";
        var result = _executor.ExecuteCommand(SupportedExecutors.ffmpeg, arguments);
        if (!string.IsNullOrEmpty(result) && result.Contains("Error:"))
        {
            throw new FFmpegException($"Error converting audio format: {result}");
        }
    }

    private static string GetFileExtensionForCodec(AudioCodec audioEncoder)
    {
        switch (audioEncoder)
        {
            case AudioCodec.AAC:
                return ".aac";
            case AudioCodec.MP3:
                return ".mp3";
            case AudioCodec.Vorbis:
                return ".ogg";
            case AudioCodec.Opus:
                return ".opus";
            case AudioCodec.FLAC:
                return ".flac";
            case AudioCodec.PCM:
                return ".pcm";
            case AudioCodec.AC3:
                return ".ac3";
            case AudioCodec.DTS:
                return ".dts";
            case AudioCodec.EAC3:
                return ".eac3";
            case AudioCodec.MP2:
                return ".mp2";
            case AudioCodec.AMR_NB:
            case AudioCodec.AMR_WB:
                return ".amr";
            case AudioCodec.WMA:
            case AudioCodec.WMAV1:
            case AudioCodec.WMAV2:
            case AudioCodec.WMAPRO:
                return ".wma";
            case AudioCodec.ALAC:
                return ".m4a";
            case AudioCodec.APE:
                return ".ape";
            case AudioCodec.DCA:
                return ".dts";
            case AudioCodec.EAC3_ATMOS:
                return ".eac3";
            case AudioCodec.TRUEHD:
                return ".thd";
            case AudioCodec.DTS_HD:
            case AudioCodec.DTS_X:
                return ".dtshd";
            case AudioCodec.G723_1:
                return ".g723";
            case AudioCodec.G729:
                return ".g729";
            case AudioCodec.GSM:
                return ".gsm";
            case AudioCodec.ILBC:
                return ".ilbc";
            case AudioCodec.ADPCM:
                return ".adpcm";
            case AudioCodec.SPEEX:
                return ".spx";
            case AudioCodec.MPC:
                return ".mpc";
            case AudioCodec.G722:
                return ".g722";
            case AudioCodec.PCM_S16BE:
            case AudioCodec.PCM_S16LE:
            case AudioCodec.PCM_S32BE:
            case AudioCodec.PCM_S32LE:
            case AudioCodec.PCM_S64BE:
            case AudioCodec.PCM_S64LE:
            case AudioCodec.PCM_F32BE:
            case AudioCodec.PCM_F32LE:
            case AudioCodec.PCM_F64BE:
            case AudioCodec.PCM_F64LE:
            case AudioCodec.PCM_U8:
            case AudioCodec.PCM_ALAW:
            case AudioCodec.PCM_MULAW:
                return ".pcm";
            default:
                throw new FormatNotFoundException();
        }
    }

    private static string AdjustOutputFilePath(string outputFilePath, string desiredExtension)
    {
        string currentExtension = Path.GetExtension(outputFilePath);
        if (!string.IsNullOrEmpty(currentExtension) && !currentExtension.Equals(desiredExtension, StringComparison.OrdinalIgnoreCase))
        {
            return Path.ChangeExtension(outputFilePath, desiredExtension);
        }
        return outputFilePath;
    }

    private static string AddExtensionIfMissing(string outputFilePath, string desiredExtension)
    {
        string currentExtension = Path.GetExtension(outputFilePath);
        if (string.IsNullOrEmpty(currentExtension))
        {
            return $"{outputFilePath}{desiredExtension}";
        }
        return outputFilePath;
    }
}