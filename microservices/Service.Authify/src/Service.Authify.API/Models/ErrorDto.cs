using System.Text.Json.Serialization;

namespace Service.Authify.API.Models;

public class ErrorDto
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("errors")]
    public Dictionary<string, string[]> Errors { get; set; }
}