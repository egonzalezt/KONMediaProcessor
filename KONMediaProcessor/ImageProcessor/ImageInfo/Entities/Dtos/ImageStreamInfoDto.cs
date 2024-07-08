namespace KONMediaProcessor.ImageProcessor.ImageInfo.Entities.Dtos;

using System.Text.Json.Serialization;

public class ImageStreamInfoDto
{
    [JsonPropertyName("width")]
    public int Width { get; set; }
    [JsonPropertyName("height")]
    public int Height { get; set; }
    [JsonPropertyName("pix_fmt")]
    public string PixFmt { get; set; }
}
