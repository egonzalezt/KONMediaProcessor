namespace KONMediaProcessor.ImageProcessor.ImageTranscoding;

using Exceptions;
using Shared;
using FFmpegExecutor;
using FileValidator;
using System.Text;
using ImageInfo.Entities;

internal class ImageTranscodingProcessor(IFFmpegExecutor executor, IFileValidator fileValidator) : IImageTranscodingProcessor
{
    private readonly IFFmpegExecutor _executor = executor;
    private readonly IFileValidator _fileValidator = fileValidator;

    public void GenerateImage(List<TextData> textDataList, string fontPath, string textColor, string backgroundColor, int width, int height, int fontSize, string outputFilePath, bool overrideFile = false, CancellationToken cancellationToken = default)
    {
        _fileValidator.EnsureDirectoryPathExists(outputFilePath);
        if (!overrideFile && _fileValidator.FileExists(outputFilePath))
        {
            throw new FileAlreadyExistsException(outputFilePath);
        }

        var command = $"-f lavfi -i color=c={backgroundColor}:s={width}x{height}:d=5 -vf \"";

        foreach (var textData in textDataList)
        {
            command += $"drawtext=text='{textData.Text}':x={textData.X}:y={textData.Y}:fontfile={fontPath}:fontcolor={textColor}:fontsize={fontSize},";
        }
        command = command.TrimEnd(',') + $"\" -frames:v 1 {outputFilePath}";
        command += overrideFile ? " -y" : " -n";
        _executor.ExecuteCommand(SupportedExecutors.ffmpeg, command, cancellationToken);
    }

    public void CombineImages(List<ImageData> imageDataList, int width, int height, string outputFilePath, string backgroundColor, bool overrideFile = false, CancellationToken cancellationToken = default)
    {
        var inputs = imageDataList.Select(i => i.Path).ToArray();
        _fileValidator.ValidatePaths(inputs, outputFilePath, overrideFile);
        var commandBuilder = new StringBuilder();
        if (overrideFile)
        {
            commandBuilder.Append("-y ");
        }

        commandBuilder.AppendFormat("-f lavfi -i color=c={0}:s={1}x{2} ", backgroundColor, width, height);
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
        commandBuilder.AppendFormat("-frames:v 1 \"{0}\"", outputFilePath);
        string command = commandBuilder.ToString();

        _executor.ExecuteCommand(SupportedExecutors.ffmpeg, command, cancellationToken);
    }

    public void ResizeImage(string inputFilePath, int newWidth, int newHeight, string outputFilePath, bool overrideFile = false, CancellationToken cancellationToken = default)
    {
        _fileValidator.ValidatePaths([inputFilePath], outputFilePath, overrideFile);
        var command = $"-i \"{inputFilePath}\" -vf scale={newWidth}:{newHeight} -frames:v 1 \"{outputFilePath}\"";
        command += overrideFile ? " -y" : " -n";
        _executor.ExecuteCommand(SupportedExecutors.ffmpeg, command, cancellationToken);
    }

    public void ConvertImageToVideo(string inputFilePath, int durationInSeconds, int width, int height, string outputFilePath, bool overrideFile = false, CancellationToken cancellationToken = default)
    {
        _fileValidator.ValidatePaths([inputFilePath], outputFilePath, overrideFile);
        var command = $"-loop 1 -i \"{inputFilePath}\" -vf \"scale={width}:{height}\" -t {durationInSeconds} -c:v libx264 -pix_fmt yuva420p \"{outputFilePath}\"";
        command += overrideFile ? " -y" : " -n";
        _executor.ExecuteCommand(SupportedExecutors.ffmpeg, command, cancellationToken);
    }

    public void ConvertImageToVideo(string inputFilePath, int durationInSeconds, string outputFilePath, bool overrideFile = false, CancellationToken cancellationToken = default)
    {
        _fileValidator.ValidatePaths(new[] { inputFilePath }, outputFilePath, overrideFile);
        var commandBuilder = new StringBuilder();
        if (overrideFile)
        {
            commandBuilder.Append("-y ");
        }
        commandBuilder.AppendFormat("-loop 1 -i \"{0}\" -vf \"scale=trunc(iw/2)*2:trunc(ih/2)*2\" -t {1} -c:v libx264 -pix_fmt yuv420p \"{2}\"",
            inputFilePath, durationInSeconds, outputFilePath);
        string command = commandBuilder.ToString();
        _executor.ExecuteCommand(SupportedExecutors.ffmpeg, command, cancellationToken);
    }

}
