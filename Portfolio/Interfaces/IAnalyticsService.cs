namespace Portfolio.Interfaces;

public interface IAnalyticsService
{
    Task TrackPageViewAsync(string pagePath, string pageTitle, string ipAddress, string userAgent, string referrer);
    Task<WebsiteAnalyticsDto> GetPageAnalyticsAsync(string pagePath);
    Task<WebsiteAnalyticsSummaryDto> GetWebsiteSummaryAsync();

    Task TrackProjectViewAsync(int projectId, string ipAddress, string userAgent, string referrer);
    Task<ProjectAnalyticsDto> GetProjectAnalyticsAsync(int projectId);
    Task<AnalyticsSummaryDto> GetProjectAnalyticsSummaryAsync();
    Task<CombinedAnalyticsSummaryDto> GetCombinedAnalyticsSummaryAsync();
}