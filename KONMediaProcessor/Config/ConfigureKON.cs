namespace KONMediaProcessor.Config;

using FFmpegExecutor;
using FileValidator;
using VideoProcessor.VideoInfo;
using VideoProcessor.VideoTranscoding;
using ImageProcessor.ImageInfo;
using ImageProcessor.ImageTranscoding;
using Microsoft.Extensions.DependencyInjection;

public static class ConfigureKON
{
    public static IServiceCollection AddKONMediaProcessor(this IServiceCollection services)
    {
        services.AddScoped<IFFmpegExecutor, FFmpegExecutor>();
        services.AddScoped<IFileValidator, FileValidator>();
        services.AddScoped<IImageInfoProcessor, ImageInfoProcessor>();
        services.AddScoped<IVideoInfoProcessor, VideoInfoProcessor>();
        services.AddScoped<IVideoTranscodingProcessor, VideoTranscodingProcessor>();
        services.AddScoped<IImageTranscodingProcessor, ImageTranscodingProcessor>();
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
