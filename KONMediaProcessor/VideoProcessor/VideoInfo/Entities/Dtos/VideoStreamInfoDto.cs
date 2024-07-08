namespace KONMediaProcessor.VideoProcessor.VideoInfo.Entities.Dtos;

using System.Text.Json.Serialization;

public class VideoStreamInfoDto
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("avg_frame_rate")]
    public string AvgFrameRate { get; set; }
}