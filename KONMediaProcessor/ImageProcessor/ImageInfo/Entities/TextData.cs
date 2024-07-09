namespace KONMediaProcessor.ImageProcessor.ImageInfo.Entities;

using Shared.Entities.Color;

public class TextData
{
    public TextData(int x, int y, string text, string? color, int fontSize)
    {
        X = x;
        Y = y;
        Text = text;
        Color = color ?? "#FFFFFF";
        FontSize = fontSize;
    }

    public int X { get; init; }
    public int Y { get; init; }
    public string Text { get; init; }
    public Color Color { get; set; }
    public int FontSize { get; set; } = 10;
}
