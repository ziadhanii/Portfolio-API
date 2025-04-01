using System.Text.Json.Serialization;

namespace Portfolio.DTOs
{
    public class WebsiteVisitDto
    {
        [JsonPropertyName("visit_date")]
        public DateTime VisitDate { get; set; }

        [JsonPropertyName("ip_address")]
        public string IpAddress { get; set; }

        [JsonPropertyName("user_agent")]
        public string UserAgent { get; set; }

        [JsonPropertyName("referrer")]
        public string Referrer { get; set; }

        [JsonPropertyName("time_spent")]
        public int? TimeSpent { get; set; }
    }

    public class WebsiteAnalyticsDto
    {
        [JsonPropertyName("page_path")]
        public string PagePath { get; set; }

        [JsonPropertyName("page_title")]
        public string PageTitle { get; set; }

        [JsonPropertyName("views_count")]
        public int ViewsCount { get; set; }

        [JsonPropertyName("unique_visits_count")]
        public int UniqueVisitsCount { get; set; }

        [JsonPropertyName("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonPropertyName("visits")]
        public List<WebsiteVisitDto> Visits { get; set; }
    }

    public class WebsiteAnalyticsSummaryDto
    {
        [JsonPropertyName("total_views")]
        public int TotalViews { get; set; }

        [JsonPropertyName("total_unique_visits")]
        public int TotalUniqueVisits { get; set; }

        [JsonPropertyName("most_visited_pages")]
        public List<WebsiteAnalyticsDto> MostVisitedPages { get; set; }

        [JsonPropertyName("recent_visits")]
        public List<WebsiteVisitDto> RecentVisits { get; set; }
    }
} 