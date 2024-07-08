namespace KONMediaProcessor.FFmpegExecutor;

using Config;
using Exceptions;
using Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Text;

internal class FFmpegExecutor : IFFmpegExecutor
{
    private readonly ILogger<FFmpegExecutor> _logger;
    private Process? _ffmpegProcess;

    public FFmpegExecutor(ILogger<FFmpegExecutor> logger)
    {
        _logger = logger;
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        Console.CancelKeyPress += OnCancelKeyPress;
    }

    public string ExecuteCommand(SupportedExecutors executor, string arguments)
    {
        _logger.LogInformation("Starting FFmpeg");

        if (string.IsNullOrEmpty(arguments))
        {
            throw new EmptyCommandException("Command arguments cannot be null");
        }

        try
        {
            var outputBuilder = new StringBuilder();
            var executorPath = executor == SupportedExecutors.ffmpeg ? FFmpegConfig.GetFFmpegLocation() : FFmpegConfig.GetFFprobeLocation();
            if (string.IsNullOrEmpty(arguments))
            {
                throw new EmptyCommandException("Command arguments cannot be null");
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = executorPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            _ffmpegProcess = new Process
            {
                StartInfo = startInfo
            };

            _ffmpegProcess.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    outputBuilder.AppendLine(e.Data);
                }
            };

            _ffmpegProcess.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    _logger.LogDebug(e.Data);
                }
            };

            _ffmpegProcess.Exited += (sender, e) =>
            {
                _logger.LogDebug("Process exited with Id: {Id}", _ffmpegProcess?.Id);
                if(_ffmpegProcess is not null)
                {
                    KillFFmpegProcess(_ffmpegProcess);
                }
            };

            _ffmpegProcess.Start();
            _ffmpegProcess.BeginOutputReadLine();
            _ffmpegProcess.BeginErrorReadLine();

            _ffmpegProcess.WaitForExit();

            if (_ffmpegProcess.ExitCode != 0)
            {
                throw new FFmpegCommandExecutionException($"Program exited with code {_ffmpegProcess.ExitCode}");
            }

            _logger.LogInformation("FFmpeg Process complete");
            var output = outputBuilder.ToString();
            return output;
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("FFmpeg process was canceled.");
            throw new FFmpegCommandExecutionException("FFmpeg process was canceled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing command: {ErrorMessage}", ex.Message);
            throw new FFmpegCommandExecutionException($"Error executing command: {ex.Message}");
        }
        finally
        {
            _ffmpegProcess?.Dispose();
        }
    }

    private void OnProcessExit(object? sender, EventArgs e)
    {
        if (_ffmpegProcess is not null)
        {
            KillFFmpegProcess(_ffmpegProcess);
        }
    }

    private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        _logger.LogInformation("Cancel key press detected. Stopping FFmpeg processes.");
        var ffmpegProcesses = Process.GetProcesses().Where(p => !p.HasExited && p.ProcessName.ToLower().Contains("ffmpeg"));
        foreach (var process in ffmpegProcesses)
        {
            try
            {
                KillFFmpegProcess(process);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Error killing process {ProcessName}: {ErrorMessage}", process.ProcessName, ex.Message);
            }
        }

        e.Cancel = true;
    }

    private void KillFFmpegProcess(Process process)
    {
        try
        {
            _logger.LogInformation("Terminating FFmpeg process with Id {Id}", process.Id);
            process.Kill();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogDebug(ex, "Error killing FFmpeg process: {ErrorMessage}", ex.Message);
        }
    }
}
