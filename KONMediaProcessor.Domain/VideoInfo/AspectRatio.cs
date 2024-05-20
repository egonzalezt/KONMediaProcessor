namespace KONMediaProcessor.Domain.VideoInfo;

public class AspectRatio
{
    public int Width { get; }
    public int Height { get; }

    public AspectRatio(int width, int height)
    {
        if (width <= 0)
        {
            throw new ArgumentException("Width must be greater than zero.", nameof(width));
        }

        if (height <= 0)
        {
            throw new ArgumentException("Height must be greater than zero.", nameof(height));
        }

        Width = width;
        Height = height;
    }

    public double GetRatio()
    {
        return (double)Width / Height;
    }

    public override string ToString()
    {
        return $"{Width}:{Height}";
    }
}
