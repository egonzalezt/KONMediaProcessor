namespace KONMediaProcessor.CustomCommandExecutor;

using Shared;
using Exceptions;
using System.Threading;

/// <summary>
/// Interface for executing custom commands using FFmpeg or FFprobe.
/// </summary>
public interface ICustomCommandExecutor
{
    /// <summary>
    /// Runs a custom command using the specified executor.
    /// </summary>
    /// <param name="arguments">The arguments for the command to be executed.</param>
    /// <param name="executor">The executor to use (FFmpeg or FFprobe). Defaults to FFmpeg.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>The output of the executed command as a string.</returns>
    /// <exception cref="FFmpegCommandExecutionException">Thrown when the operation is cancelled or an error occurs running the command</exception>

    string RunCommand(string arguments, SupportedExecutors executor = SupportedExecutors.ffmpeg, CancellationToken cancellationToken = default);
}