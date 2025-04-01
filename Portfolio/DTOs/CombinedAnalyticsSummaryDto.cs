using System.Text.Json.Serialization;

namespace Portfolio.DTOs;

public class CombinedAnalyticsSummaryDto
{
    [JsonPropertyName("website_analytics")]
    public WebsiteAnalyticsSummaryDto WebsiteAnalytics { get; set; }

    [JsonPropertyName("project_analytics")]
    public AnalyticsSummaryDto ProjectAnalytics { get; set; }

    [JsonPropertyName("total_combined_views")]
    public int TotalCombinedViews { get; set; }

    [JsonPropertyName("total_combined_visits")]
    public int TotalCombinedVisits { get; set; }

    [JsonPropertyName("last_updated")]
    public DateTime LastUpdated { get; set; }
} 