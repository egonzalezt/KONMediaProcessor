namespace KONMediaProcessor.VideoProcessor.VideoInfo.Entities.Dtos;

using System.Text.Json.Serialization;

public class FFprobeResultDto
{
    [JsonPropertyName("streams")]
    public List<VideoStreamInfoDto> Streams { get; set; }
}
