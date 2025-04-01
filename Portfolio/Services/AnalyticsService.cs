using Microsoft.EntityFrameworkCore;
using Portfolio.DTOs;
using Portfolio.Entities;

namespace Portfolio.Services;

public interface IAnalyticsService
{
    Task TrackProjectViewAsync(int projectId, string ipAddress, string userAgent, string referrer);
    Task<ProjectAnalyticsDto> GetProjectAnalyticsAsync(int projectId);
    Task<AnalyticsSummaryDto> GetAnalyticsSummaryAsync();
}

public class AnalyticsService : IAnalyticsService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AnalyticsService> _logger;

    public AnalyticsService(
        ApplicationDbContext context,
        ILogger<AnalyticsService> logger)
    {
        _context = context;
        _logger = logger;
    }

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

            // Increment views count
            analytics.ViewsCount++;
            analytics.LastUpdated = DateTime.UtcNow;

            // Check if this is a unique visit (based on IP address)
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

    public async Task<AnalyticsSummaryDto> GetAnalyticsSummaryAsync()
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
} 