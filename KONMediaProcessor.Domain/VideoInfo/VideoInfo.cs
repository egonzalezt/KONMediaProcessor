namespace KONMediaProcessor.Domain.VideoInfo;

public class VideoInfo
{
    public int Width { get; set; }
    public int Height { get; set; }
    public double FrameRate { get; set; }

    public override string ToString()
    {
        return $"Width: {Width}, Height: {Height}, FrameRate: {FrameRate} fps";
    }
}
