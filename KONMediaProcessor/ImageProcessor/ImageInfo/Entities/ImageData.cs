namespace KONMediaProcessor.ImageProcessor.ImageInfo.Entities;

public class ImageData
{
    public ImageData(string path, int x, int y)
    {
        Path = System.IO.Path.GetFullPath(path);
        X = x;
        Y = y;
    }

    public string Path { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
}
