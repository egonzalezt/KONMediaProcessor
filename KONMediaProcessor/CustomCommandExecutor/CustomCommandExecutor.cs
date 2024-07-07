namespace KONMediaProcessor.CustomCommandExecutor;

using Domain.Shared;
using FFmpegExecutor;

internal class CustomCommandExecutor(IFFmpegExecutor ffmpegExecutor) : ICustomCommandExecutor
{
    public string RunCommand(string arguments, SupportedExecutors executor = SupportedExecutors.ffmpeg, CancellationToken cancellationToken = default)
    {
        return ffmpegExecutor.ExecuteCommand(executor, arguments, cancellationToken);
    }
}
