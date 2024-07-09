// See https://aka.ms/new-console-template for more information
using KONMediaProcessor.Config;
using KONMediaProcessor.Sample.Examples;
using KONMediaProcessor.Sample.Examples.Image;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

FFmpegConfig.logCommand = true;
var serviceCollection = new ServiceCollection();
ConfigureServices(serviceCollection);
var serviceProvider = serviceCollection.BuildServiceProvider();

var imageInfoSamples = serviceProvider.GetRequiredService<ImageInfoSamples>();
imageInfoSamples.GetImageInformation();

var imageTranscodingSamples = serviceProvider.GetRequiredService<ImageTranscodingSamples>();
imageTranscodingSamples.ImageAsBase64();
imageTranscodingSamples.CreateImage();
imageTranscodingSamples.JoinImages();
imageTranscodingSamples.ResizeImage();
imageTranscodingSamples.ImageToVideo();

var videoInfoSamples = serviceProvider.GetRequiredService<VideoInfoSamples>();
videoInfoSamples.GetVideoInformation();

var videoTranscodingSamples = serviceProvider.GetRequiredService<VideoTranscodingSamples>();
videoTranscodingSamples.TranscodeVideo();
videoTranscodingSamples.ChangeVideoResolution();
videoTranscodingSamples.SetVideoFrameRate();
videoTranscodingSamples.ConcatenateVideos();
videoTranscodingSamples.ChangeAspectRatio();

var audioInfoSamples = serviceProvider.GetRequiredService<AudioInfoSamples>();
audioInfoSamples.GetAudioInfo();
audioInfoSamples.GetAudioInfoFromVideo();

var audioTranscodingSamples = serviceProvider.GetRequiredService<AudioTranscodingSamples>();
audioTranscodingSamples.TranscodeAudio();
audioTranscodingSamples.ChangeBitRate();
audioTranscodingSamples.ChangeChannels();
audioTranscodingSamples.ConvertAudioFormat();

static void ConfigureServices(ServiceCollection services)
{

    services.AddLogging(configure =>
    {
        configure.AddConsole();
        configure.SetMinimumLevel(LogLevel.Debug);
    });
    services.AddKONMediaProcessor();
    services
        .AddSingleton<ImageInfoSamples>()
        .AddSingleton<ImageTranscodingSamples>()
        .AddSingleton<VideoInfoSamples>()
        .AddSingleton<VideoTranscodingSamples>()
        .AddSingleton<AudioInfoSamples>()
        .AddSingleton<AudioTranscodingSamples>();
}
