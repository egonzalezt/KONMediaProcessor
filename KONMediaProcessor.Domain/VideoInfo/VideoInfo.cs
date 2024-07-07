namespace KONMediaProcessor.Domain.VideoInfo;

public class VideoInfo
{
    public int Width { get; init; }
    public int Height { get; init; }
    public double FrameRate { get; init; }

    public override string ToString()
    {
        return $"Width: {Width}, Height: {Height}, FrameRate: {FrameRate} fps";
    }
}
