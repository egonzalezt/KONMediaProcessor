namespace KONMediaProcessor.Config;

using FFmpegExecutor;
using FileValidator;
using VideoProcessor.VideoInfo;
using VideoProcessor.VideoTranscoding;
using ImageProcessor.ImageInfo;
using ImageProcessor.ImageTranscoding;
using AudioProcessor.AudioInfo;
using AudioProcessor.AudioTranscoding;
using CustomCommandExecutor;
using Microsoft.Extensions.DependencyInjection;

public static class ConfigureKON
{
    public static IServiceCollection AddKONMediaProcessor(this IServiceCollection services)
    {
        services.AddScoped<IFFmpegExecutor, FFmpegExecutor>()
        .AddScoped<IFileValidator, FileValidator>()
        .AddScoped<IImageInfoProcessor, ImageInfoProcessor>()
        .AddScoped<IVideoInfoProcessor, VideoInfoProcessor>()
        .AddScoped<IVideoTranscodingProcessor, VideoTranscodingProcessor>()
        .AddScoped<IImageTranscodingProcessor, ImageTranscodingProcessor>()
        .AddScoped<ICustomCommandExecutor, CustomCommandExecutor>()
        .AddScoped<IAudioInfoProcessor, AudioInfoProcessor>()
        .AddScoped<IAudioTranscodingProcessor, AudioTranscodingProcessor>();
        ConfigureFFmpegAndFFprobe();
        return services;
    }

    private static void ConfigureFFmpegAndFFprobe()
    {
        if (string.IsNullOrEmpty(FFmpegConfig.GetFFmpegLocation()))
        {
            var ffmpegLocation = FFmpegConfigBridge.LocateFFmpeg();
            FFmpegConfig.SetFFmpegLocation(ffmpegLocation);
        }
        if (string.IsNullOrEmpty(FFmpegConfig.GetFFprobeLocation()))
        {
            var ffprobeLocation = FFmpegConfigBridge.LocateFFprobe();
            FFmpegConfig.SetFFprobeLocation(ffprobeLocation);
        }

        FFmpegConfigBridge.CheckIfFFmpegExists();
        FFmpegConfigBridge.CheckIfFFprobeExists();
    }
}
