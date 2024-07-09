namespace KONMediaProcessor.ImageProcessor.ImageInfo.Entities;

using Shared.Entities.Color;

public class Canvas(int width, int height, Color backgroundColor, string path)
{
    public Color BackgroundColor { get; set; } = backgroundColor;
    public int Width { get; set; } = width;
    public int Height { get; set; } = height;
    public string Path { get; set; } = System.IO.Path.GetFullPath(path);
}
