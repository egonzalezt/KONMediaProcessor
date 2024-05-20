namespace KONMediaProcessor.FFmpegExecutor;

using KONMediaProcessor.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Text;

internal class FFmpegExecutor(ILogger<FFmpegExecutor> logger) : IFFmpegExecutor
{
    private readonly ILogger<FFmpegExecutor> _logger = logger;

    public string ExecuteCommand(string commandPath, string arguments)
    {
        _logger.LogInformation("Starting FFmpeg");
        try
        {
            var outputBuilder = new StringBuilder();

            var startInfo = new ProcessStartInfo
            {
                FileName = commandPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = startInfo;

            process.OutputDataReceived += (sender, e) =>
            {
                var outputData = e.Data;
                if (!string.IsNullOrEmpty(outputData))
                {
                    outputBuilder.AppendLine(outputData);
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                var errorData = e.Data;
                if (!string.IsNullOrEmpty(errorData))
                {
                    _logger.LogDebug(errorData);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            var output = outputBuilder.ToString();

            if (process.ExitCode != 0)
            {
                throw new FFmpegCommandExecutionException($"Program exited");
            }
            _logger.LogInformation("FFmpeg Process complete");
            return output;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing command: {ErrorMessage}", ex.Message);
            throw new FFmpegCommandExecutionException($"Error executing command: {ex.Message}");
        }
    }
}
