using Portfolio.DTOs;
using Portfolio.Entities;

namespace Portfolio.Services
{
    public interface IWebsiteAnalyticsService
    {
        Task TrackPageViewAsync(string pagePath, string pageTitle, string ipAddress, string userAgent, string referrer);
        Task<WebsiteAnalyticsDto> GetPageAnalyticsAsync(string pagePath);
        Task<WebsiteAnalyticsSummaryDto> GetWebsiteSummaryAsync();
    }

    public class WebsiteAnalyticsService : IWebsiteAnalyticsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WebsiteAnalyticsService> _logger;

        public WebsiteAnalyticsService(ApplicationDbContext context, ILogger<WebsiteAnalyticsService> logger)
        {
            _context = context;
            _logger = logger;
        }

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
    }
} 