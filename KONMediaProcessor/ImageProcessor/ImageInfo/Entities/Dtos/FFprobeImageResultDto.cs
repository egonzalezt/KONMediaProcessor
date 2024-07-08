namespace KONMediaProcessor.ImageProcessor.ImageInfo.Entities.Dtos;

using System.Text.Json.Serialization;

public class FFprobeImageResultDto
{
    [JsonPropertyName("streams")]
    public List<ImageStreamInfoDto> Streams { get; set; }
}
