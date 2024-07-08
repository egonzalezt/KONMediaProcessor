namespace KONMediaProcessor.Sample.Examples;

using Microsoft.Extensions.Logging;
using VideoProcessor.VideoInfo;

public class VideoInfoSamples(IVideoInfoProcessor videoInfoProcessor, ILogger<VideoInfoSamples> logger)
{
    const string sampleVideoPath = "Examples/Multimedia/sampleVideo.mp4";

    public void GetVideoInformation()
    {
        var response = videoInfoProcessor.GetVideoInfo(sampleVideoPath);
        logger.LogInformation("With: {W}, Height: {H}, FrameRate: {F}", response.Width, response.Height, response.FrameRate);
    }
}
