namespace KONMediaProcessor.CustomCommandExecutor;

using KONMediaProcessor.Config;
using KONMediaProcessor.FFmpegExecutor;

internal class CustomCommandExecutor(IFFmpegExecutor ffmpegExecutor) : ICustomCommandExecutor
{
    public string RunCommand(string arguments, CancellationToken cancellationToken = default)
    {
        return ffmpegExecutor.ExecuteCommand(FFmpegConfig.GetFFmpegLocation(), arguments, cancellationToken);
    }
}
