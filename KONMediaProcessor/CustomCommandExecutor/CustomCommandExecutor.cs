namespace KONMediaProcessor.CustomCommandExecutor;

using Shared;
using FFmpegExecutor;

internal class CustomCommandExecutor(IFFmpegExecutor ffmpegExecutor) : ICustomCommandExecutor
{
    public string RunCommand(string arguments, SupportedExecutors executor = SupportedExecutors.ffmpeg)
    {
        return ffmpegExecutor.ExecuteCommand(executor, arguments);
    }
}
