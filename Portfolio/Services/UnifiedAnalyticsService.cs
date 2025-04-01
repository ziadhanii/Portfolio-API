using Microsoft.EntityFrameworkCore;
using Portfolio.DTOs;
using Portfolio.Entities;

namespace Portfolio.Services;

public interface IUnifiedAnalyticsService
{
    // Website Analytics
    Task TrackPageViewAsync(string pagePath, string pageTitle, string ipAddress, string userAgent, string referrer);
    Task<WebsiteAnalyticsDto> GetPageAnalyticsAsync(string pagePath);
    Task<WebsiteAnalyticsSummaryDto> GetWebsiteSummaryAsync();

    // Project Analytics
    Task TrackProjectViewAsync(int projectId, string ipAddress, string userAgent, string referrer);
    Task<ProjectAnalyticsDto> GetProjectAnalyticsAsync(int projectId);
    Task<AnalyticsSummaryDto> GetProjectAnalyticsSummaryAsync();

    // Combined Analytics
    Task<CombinedAnalyticsSummaryDto> GetCombinedAnalyticsSummaryAsync();
}

public class UnifiedAnalyticsService : IUnifiedAnalyticsService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UnifiedAnalyticsService> _logger;

    public UnifiedAnalyticsService(
        ApplicationDbContext context,
        ILogger<UnifiedAnalyticsService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Website Analytics Methods
    public async Task TrackPageViewAsync(string pagePath, string pageTitle, string ipAddress, string userAgent, string referrer)
    {
        try
        {
            var analytics = await _context.WebsiteAnalytics
                .Include(a => a.Visits)
                .FirstOrDefaultAsync(a => a.PagePath == pagePath);

            if (analytics == null)
            {
                analytics = new WebsiteAnalytics
                {
                    PagePath = pagePath,
                    PageTitle = pageTitle,
                    ViewsCount = 0,
                    UniqueVisitsCount = 0,
                    LastUpdated = DateTime.UtcNow,
                    Visits = new List<WebsiteVisit>()
                };
                _context.WebsiteAnalytics.Add(analytics);
            }

            var visit = new WebsiteVisit
            {
                PagePath = pagePath,
                PageTitle = pageTitle,
                VisitDate = DateTime.UtcNow,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Referrer = referrer
            };

            analytics.Visits.Add(visit);
            analytics.ViewsCount++;
            analytics.UniqueVisitsCount = analytics.Visits.Select(v => v.IpAddress).Distinct().Count();
            analytics.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking page view for {PagePath}", pagePath);
            throw;
        }
    }

    public async Task<WebsiteAnalyticsDto> GetPageAnalyticsAsync(string pagePath)
    {
        var analytics = await _context.WebsiteAnalytics
            .Include(a => a.Visits)
            .FirstOrDefaultAsync(a => a.PagePath == pagePath);

        if (analytics == null)
            return null;

        return new WebsiteAnalyticsDto
        {
            PagePath = analytics.PagePath,
            PageTitle = analytics.PageTitle,
            ViewsCount = analytics.ViewsCount,
            UniqueVisitsCount = analytics.UniqueVisitsCount,
            LastUpdated = analytics.LastUpdated,
            Visits = analytics.Visits.Select(v => new WebsiteVisitDto
            {
                VisitDate = v.VisitDate,
                IpAddress = v.IpAddress,
                UserAgent = v.UserAgent,
                Referrer = v.Referrer,
                TimeSpent = v.TimeSpent
            }).ToList()
        };
    }

    public async Task<WebsiteAnalyticsSummaryDto> GetWebsiteSummaryAsync()
    {
        var allAnalytics = await _context.WebsiteAnalytics
            .Include(a => a.Visits)
            .ToListAsync();

        var recentVisits = await _context.WebsiteVisits
            .OrderByDescending(v => v.VisitDate)
            .Take(10)
            .Select(v => new WebsiteVisitDto
            {
                VisitDate = v.VisitDate,
                IpAddress = v.IpAddress,
                UserAgent = v.UserAgent,
                Referrer = v.Referrer,
                TimeSpent = v.TimeSpent
            })
            .ToListAsync();

        return new WebsiteAnalyticsSummaryDto
        {
            TotalViews = allAnalytics.Sum(a => a.ViewsCount),
            TotalUniqueVisits = allAnalytics.Sum(a => a.UniqueVisitsCount),
            MostVisitedPages = allAnalytics
                .OrderByDescending(a => a.ViewsCount)
                .Take(5)
                .Select(a => new WebsiteAnalyticsDto
                {
                    PagePath = a.PagePath,
                    PageTitle = a.PageTitle,
                    ViewsCount = a.ViewsCount,
                    UniqueVisitsCount = a.UniqueVisitsCount,
                    LastUpdated = a.LastUpdated,
                    Visits = a.Visits.Select(v => new WebsiteVisitDto
                    {
                        VisitDate = v.VisitDate,
                        IpAddress = v.IpAddress,
                        UserAgent = v.UserAgent,
                        Referrer = v.Referrer,
                        TimeSpent = v.TimeSpent
                    }).ToList()
                })
                .ToList(),
            RecentVisits = recentVisits
        };
    }

    // Project Analytics Methods
    public async Task TrackProjectViewAsync(int projectId, string ipAddress, string userAgent, string referrer)
    {
        try
        {
            var analytics = await _context.ProjectAnalytics
                .Include(a => a.Project)
                .FirstOrDefaultAsync(a => a.ProjectId == projectId);

            if (analytics == null)
            {
                analytics = new ProjectAnalytics
                {
                    ProjectId = projectId,
                    ViewsCount = 0,
                    VisitsCount = 0,
                    LastUpdated = DateTime.UtcNow
                };
                _context.ProjectAnalytics.Add(analytics);
            }

            analytics.ViewsCount++;
            analytics.LastUpdated = DateTime.UtcNow;

            var isUniqueVisit = !analytics.Visits.Any(v => v.IpAddress == ipAddress);
            if (isUniqueVisit)
            {
                analytics.VisitsCount++;
                analytics.Visits.Add(new ProjectVisit
                {
                    VisitDate = DateTime.UtcNow,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    Referrer = referrer
                });
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking project view for project {ProjectId}", projectId);
            throw;
        }
    }

    public async Task<ProjectAnalyticsDto> GetProjectAnalyticsAsync(int projectId)
    {
        var analytics = await _context.ProjectAnalytics
            .Include(a => a.Project)
            .Include(a => a.Visits)
            .FirstOrDefaultAsync(a => a.ProjectId == projectId);

        if (analytics == null)
            return null;

        return new ProjectAnalyticsDto
        {
            ProjectId = analytics.ProjectId,
            ProjectTitle = analytics.Project.Title,
            ViewsCount = analytics.ViewsCount,
            VisitsCount = analytics.VisitsCount,
            LastUpdated = analytics.LastUpdated,
            Visits = analytics.Visits.Select(v => new ProjectVisitDto
            {
                VisitDate = v.VisitDate,
                IpAddress = v.IpAddress,
                UserAgent = v.UserAgent,
                Referrer = v.Referrer
            }).ToList()
        };
    }

    public async Task<AnalyticsSummaryDto> GetProjectAnalyticsSummaryAsync()
    {
        var analytics = await _context.ProjectAnalytics
            .Include(a => a.Project)
            .Include(a => a.Visits)
            .ToListAsync();

        var totalViews = analytics.Sum(a => a.ViewsCount);
        var totalVisits = analytics.Sum(a => a.VisitsCount);

        var mostViewedProjects = analytics
            .OrderByDescending(a => a.ViewsCount)
            .Take(5)
            .Select(a => new ProjectAnalyticsDto
            {
                ProjectId = a.ProjectId,
                ProjectTitle = a.Project.Title,
                ViewsCount = a.ViewsCount,
                VisitsCount = a.VisitsCount,
                LastUpdated = a.LastUpdated
            })
            .ToList();

        var recentVisits = analytics
            .SelectMany(a => a.Visits)
            .OrderByDescending(v => v.VisitDate)
            .Take(10)
            .Select(v => new ProjectVisitDto
            {
                VisitDate = v.VisitDate,
                IpAddress = v.IpAddress,
                UserAgent = v.UserAgent,
                Referrer = v.Referrer
            })
            .ToList();

        return new AnalyticsSummaryDto
        {
            TotalViews = totalViews,
            TotalVisits = totalVisits,
            MostViewedProjects = mostViewedProjects,
            RecentVisits = recentVisits
        };
    }

    // Combined Analytics Method
    public async Task<CombinedAnalyticsSummaryDto> GetCombinedAnalyticsSummaryAsync()
    {
        var websiteSummary = await GetWebsiteSummaryAsync();
        var projectSummary = await GetProjectAnalyticsSummaryAsync();

        return new CombinedAnalyticsSummaryDto
        {
            WebsiteAnalytics = websiteSummary,
            ProjectAnalytics = projectSummary,
            TotalCombinedViews = websiteSummary.TotalViews + projectSummary.TotalViews,
            TotalCombinedVisits = websiteSummary.TotalUniqueVisits + projectSummary.TotalVisits,
            LastUpdated = DateTime.UtcNow
        };
    }
} 