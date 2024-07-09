namespace KONMediaProcessor.ImageProcessor.ImageTranscoding;

using Shared;
using FFmpegExecutor;
using FileValidator;
using System.Text;
using ImageInfo.Entities;
using System.Runtime.InteropServices;

internal class ImageTranscodingProcessor(IFFmpegExecutor executor, IFileValidator fileValidator) : IImageTranscodingProcessor
{
    private readonly IFFmpegExecutor _executor = executor;
    private readonly IFileValidator _fileValidator = fileValidator;

    public void GenerateImage(List<TextData> textDataList, Canvas canvas, string? fontPath = null, bool overrideFile = false)
    {
        var processedOutputPath = _fileValidator.ValidateOutputPath(canvas.Path, overrideFile);

        if (!string.IsNullOrEmpty(fontPath))
        {
            fontPath = _fileValidator.ValidateFileExists(fontPath);
            fontPath = EscapeFontPath(fontPath);
        }

        var command = $"-f lavfi -i color=c={(string)canvas.BackgroundColor}:s={canvas.Width}x{canvas.Height}:d=5 -vf \"";

        foreach (var textData in textDataList)
        {
            var escapedText = EscapeFFmpegText(textData.Text);

            if (!string.IsNullOrEmpty(fontPath))
            {
                command += $"drawtext=text='{escapedText}':x={textData.X}:y={textData.Y}:fontfile='{fontPath}':fontcolor={(string)textData.Color}:fontsize={textData.FontSize},";
            }
            else
            {
                command += $"drawtext=text='{escapedText}':x={textData.X}:y={textData.Y}:fontcolor={(string)textData.Color}:fontsize={textData.FontSize},";
            }
        }
        command = command.TrimEnd(',') + $"\" -frames:v 1 {processedOutputPath}";
        command += overrideFile ? " -y" : " -n";
        _executor.ExecuteCommand(SupportedExecutors.ffmpeg, command);
    }

    private static string EscapeFFmpegText(string text)
    {
        return text.Replace("\\", "\\\\")
                   .Replace("'", "\\'")
                   .Replace("\"", "\\\"")
                   .Replace(":", "\\:");
    }

    private static string EscapeFontPath(string fontPath)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return fontPath.Replace(@"\", @"\\").Replace(":", "\\:");
        }
        else
        {
            return fontPath.Replace(":", "\\:");
        }
    }

    public void CombineImages(List<ImageData> imageDataList, Canvas canvas, bool overrideFile = false)
    {
        var inputs = imageDataList.Select(i => i.Path).ToArray();
        (_, string outputPath) = _fileValidator.ValidatePaths(inputs, canvas.Path, overrideFile);
        var commandBuilder = new StringBuilder();
        if (overrideFile)
        {
            commandBuilder.Append("-y ");
        }

        commandBuilder.AppendFormat("-f lavfi -i color=c={0}:s={1}x{2} ", (string)canvas.BackgroundColor, canvas.Width, canvas.Height);
        for (int i = 0; i < imageDataList.Count; i++)
        {
            commandBuilder.AppendFormat("-i \"{0}\" ", imageDataList[i].Path);
        }

        commandBuilder.Append("-filter_complex \"");
        for (int i = 0; i < imageDataList.Count; i++)
        {
            if (i == 0)
            {
                commandBuilder.AppendFormat("[0][{1}]overlay={2}:{3}[tmp{1}]; ", i + 1, i + 1, imageDataList[i].X, imageDataList[i].Y);
            }
            else if (i < imageDataList.Count - 1)
            {
                commandBuilder.AppendFormat("[tmp{0}][{1}]overlay={2}:{3}[tmp{1}]; ", i, i + 1, imageDataList[i].X, imageDataList[i].Y);
            }
            else
            {
                commandBuilder.AppendFormat("[tmp{0}][{1}]overlay={2}:{3}", i, i + 1, imageDataList[i].X, imageDataList[i].Y);
            }
        }

        commandBuilder.Append("\" ");
        commandBuilder.AppendFormat("-frames:v 1 \"{0}\"", outputPath);
        string command = commandBuilder.ToString();

        _executor.ExecuteCommand(SupportedExecutors.ffmpeg, command);
    }

    public void ResizeImage(string inputFilePath, int newWidth, int newHeight, string outputFilePath, bool overrideFile = false)
    {
        var (validatedInputs, validatedOutput) = _fileValidator.ValidatePaths([inputFilePath], outputFilePath, overrideFile);
        var command = $"-i \"{validatedInputs.First()}\" -vf scale={newWidth}:{newHeight} -frames:v 1 \"{validatedOutput}\"";
        command += overrideFile ? " -y" : " -n";
        _executor.ExecuteCommand(SupportedExecutors.ffmpeg, command);
    }

    public void ConvertImageToVideo(string inputFilePath, int durationInSeconds, int width, int height, string outputFilePath, bool overrideFile = false)
    {
        var (validatedInputs, validatedOutput) = _fileValidator.ValidatePaths([inputFilePath], outputFilePath, overrideFile);
        var command = $"-loop 1 -i \"{validatedInputs.First()}\" -vf \"scale={width}:{height}\" -t {durationInSeconds} -c:v libx264 -pix_fmt yuva420p \"{validatedOutput}\"";
        command += overrideFile ? " -y" : " -n";
        _executor.ExecuteCommand(SupportedExecutors.ffmpeg, command);
    }

    public void ConvertImageToVideo(string inputFilePath, int durationInSeconds, string outputFilePath, bool overrideFile = false)
    {
        var (validatedInputs, validatedOutput) = _fileValidator.ValidatePaths([inputFilePath], outputFilePath, overrideFile);
        var commandBuilder = new StringBuilder();
        if (overrideFile)
        {
            commandBuilder.Append("-y ");
        }
        commandBuilder.AppendFormat("-loop 1 -i \"{0}\" -vf \"scale=trunc(iw/2)*2:trunc(ih/2)*2\" -t {1} -c:v libx264 -pix_fmt yuv420p \"{2}\"",
            validatedInputs.First(), durationInSeconds, validatedOutput);
        string command = commandBuilder.ToString();
        _executor.ExecuteCommand(SupportedExecutors.ffmpeg, command);
    }

    public string ImageToBase64(string imagePath)
    {
        var processedPath = _fileValidator.ValidateFileExists(imagePath);
        byte[] imageBytes = File.ReadAllBytes(processedPath);
        string base64String = Convert.ToBase64String(imageBytes);
        return base64String;
    }
}
