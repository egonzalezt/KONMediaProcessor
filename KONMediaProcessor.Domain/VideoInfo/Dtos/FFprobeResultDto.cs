namespace KONMediaProcessor.Domain.VideoInfo.Dtos;

using System.Text.Json.Serialization;

public class FFprobeResultDto
{
    [JsonPropertyName("streams")]
    public List<VideoStreamInfoDto> Streams { get; set; }
}
