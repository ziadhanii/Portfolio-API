using System.Text.Json.Serialization;

namespace Portfolio.DTOs;

public class TrackPageViewRequest
{
    [JsonPropertyName("page_path")]
    public string PagePath { get; set; }

    [JsonPropertyName("page_title")]
    public string PageTitle { get; set; }
} 