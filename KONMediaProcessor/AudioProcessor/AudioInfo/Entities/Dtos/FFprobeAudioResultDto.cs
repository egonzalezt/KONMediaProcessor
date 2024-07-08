namespace KONMediaProcessor.AudioProcessor.AudioInfo.Entities.Dtos;

using System.Text.Json.Serialization;

public class FFprobeAudioResultDto
{
    [JsonPropertyName("streams")]
    public List<AudioStreamDto> Streams { get; set; }
}
