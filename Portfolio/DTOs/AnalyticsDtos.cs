using System.Text.Json.Serialization;

namespace Portfolio.DTOs;

public class ProjectAnalyticsDto
{
    [JsonPropertyName("project_id")]
    public int ProjectId { get; set; }

    [JsonPropertyName("project_title")]
    public string ProjectTitle { get; set; }

    [JsonPropertyName("views_count")]
    public int ViewsCount { get; set; }

    [JsonPropertyName("visits_count")]
    public int VisitsCount { get; set; }

    [JsonPropertyName("last_updated")]
    public DateTime LastUpdated { get; set; }

    [JsonPropertyName("visits")]
    public List<ProjectVisitDto> Visits { get; set; }
}

public class ProjectVisitDto
{
    [JsonPropertyName("visit_date")]
    public DateTime VisitDate { get; set; }

    [JsonPropertyName("ip_address")]
    public string IpAddress { get; set; }

    [JsonPropertyName("user_agent")]
    public string UserAgent { get; set; }

    [JsonPropertyName("referrer")]
    public string Referrer { get; set; }
}

public class AnalyticsSummaryDto
{
    [JsonPropertyName("total_views")]
    public int TotalViews { get; set; }

    [JsonPropertyName("total_visits")]
    public int TotalVisits { get; set; }

    [JsonPropertyName("most_viewed_projects")]
    public List<ProjectAnalyticsDto> MostViewedProjects { get; set; }

    [JsonPropertyName("recent_visits")]
    public List<ProjectVisitDto> RecentVisits { get; set; }
} 