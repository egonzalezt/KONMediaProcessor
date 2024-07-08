namespace KONMediaProcessor.AudioProcessor.AudioInfo.Entities.Dtos;

using System.Text.Json.Serialization;

public class AudioStreamDto
{
    [JsonPropertyName("codec_name")]
    public string CodecName { get; set; }

    [JsonPropertyName("sample_rate")]
    public string SampleRate { get; set; }

    [JsonPropertyName("channels")]
    public int Channels { get; set; }
}
