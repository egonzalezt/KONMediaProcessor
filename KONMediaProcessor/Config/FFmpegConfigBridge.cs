namespace KONMediaProcessor.Config;

using Exceptions;
using Shared;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public static class FFmpegConfigBridge
{
    private const string WindowsProgramLocatorCommand = "where";
    private const string UnixProgramLocatorCommand = "which";
    private const string FFmpegVersionCommand = "-version";

    public static string LocateFFmpeg()
    {
        var ffmpegExecutableName = SupportedExecutors.ffmpeg.ToString();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var ffmpegPath = LocateExecutable(WindowsProgramLocatorCommand, ffmpegExecutableName);
            if (!string.IsNullOrEmpty(ffmpegPath))
                return ffmpegPath;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var ffmpegPath = LocateExecutable(UnixProgramLocatorCommand, ffmpegExecutableName);
            if (!string.IsNullOrEmpty(ffmpegPath))
                return ffmpegPath;
        }

        throw new FFmpegNotFoundException("Unable to locate ffmpeg executable.");
    }

    public static string LocateFFprobe()
    {
        var ffprobeExecutableName = SupportedExecutors.ffprobe.ToString();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var ffprobePath = LocateExecutable(WindowsProgramLocatorCommand, ffprobeExecutableName);
            if (!string.IsNullOrEmpty(ffprobePath))
                return ffprobePath;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var ffprobePath = LocateExecutable(UnixProgramLocatorCommand, ffprobeExecutableName);
            if (!string.IsNullOrEmpty(ffprobePath))
                return ffprobePath;
        }

        throw new FFmpegNotFoundException("Unable to locate ffprobe executable.");
    }

    public static void CheckIfFFmpegExists()
    {
        var ffmpegPath = FFmpegConfig.GetFFmpegLocation();
        LocateExecutable(ffmpegPath, FFmpegVersionCommand);
    }

    public static void CheckIfFFprobeExists()
    {
        var ffprobePath = FFmpegConfig.GetFFprobeLocation();
        LocateExecutable(ffprobePath, FFmpegVersionCommand);
    }


    private static string LocateExecutable(string command, string executable)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = executable,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            using var process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
                if (process.ExitCode == 0)
                {
                    return process.StandardOutput.ReadToEnd().Trim();
                }
                else
                {
                    throw new FFmpegNotFoundException($"Failed to locate {executable} executable. Process exited with code {process.ExitCode}.");
                }
            }
            throw new FFmpegNotFoundException($"Failed to start {command} running the command {executable}.");
        }
        catch (FFmpegNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new FFmpegNotFoundException($"Failed to start {command} running the command '{executable}'.", ex);
        }
    }
}
