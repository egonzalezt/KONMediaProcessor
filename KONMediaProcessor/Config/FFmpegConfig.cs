namespace KONMediaProcessor.Config;

public static class FFmpegConfig
{
    private static string ffmpegLocation;
    private static string ffprobeLocation;
    public static bool logCommand { get; set; } = false;
    public static void SetFFmpegLocation(string location)
    {
        ffmpegLocation = location;
    }

    public static string GetFFmpegLocation()
    {
        return ffmpegLocation;
    }

    public static void SetFFprobeLocation(string location)
    {
        ffprobeLocation = location;
    }

    public static string GetFFprobeLocation()
    {
        return ffprobeLocation;
    }
}
