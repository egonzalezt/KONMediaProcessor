namespace KONMediaProcessor.Domain.AudioInfo.Dtos;

using System.Text.Json.Serialization;

public class FFprobeAudioResultDto
{
    [JsonPropertyName("streams")]
    public List<AudioStreamDto> Streams { get; set; }
}
