namespace Portfolio.Controllers;

public class AnalyticsController(
    IAnalyticsService analyticsService,
    ILogger<AnalyticsController> logger,
    ApplicationDbContext context)
    : BaseApiController
{
    [HttpPost("website/track-view")]
    public async Task<IActionResult> TrackPageView([FromBody] TrackPageViewRequest request)
    {
        try
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers.UserAgent.ToString();
            var referrer = Request.Headers.Referer.ToString();

            await analyticsService.TrackPageViewAsync(
                request.PagePath,
                request.PageTitle,
                ipAddress,
                userAgent,
                referrer);

            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error tracking page view");
            return StatusCode(500, "Error tracking page view");
        }
    }

    [HttpGet("website/page/{pagePath}")]
    public async Task<ActionResult<WebsiteAnalyticsDto>> GetPageAnalytics(string pagePath)
    {
        try
        {
            var analytics = await analyticsService.GetPageAnalyticsAsync(pagePath);
            if (analytics == null)
                return NotFound();

            return Ok(analytics);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting page analytics for {PagePath}", pagePath);
            return StatusCode(500, "Error getting page analytics");
        }
    }

    [HttpGet("website/summary")]
    public async Task<ActionResult<WebsiteAnalyticsSummaryDto>> GetWebsiteSummary()
    {
        try
        {
            var summary = await analyticsService.GetWebsiteSummaryAsync();
            return Ok(summary);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting website summary");
            return StatusCode(500, "Error getting website summary");
        }
    }

    [HttpPost("project/track-view/{projectId:int}")]
    public async Task<IActionResult> TrackProjectView(int projectId)
    {
        try
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers["User-Agent"].ToString();
            var referrer = Request.Headers["Referer"].ToString();

            await analyticsService.TrackProjectViewAsync(projectId, ipAddress, userAgent, referrer);
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error tracking project view for project {ProjectId}", projectId);
            return StatusCode(500, "Failed to track project view");
        }
    }

    [HttpGet("project/{projectId:int}")]
    public async Task<ActionResult<ProjectAnalyticsDto>> GetProjectAnalytics(int projectId)
    {
        try
        {
            var analytics = await analyticsService.GetProjectAnalyticsAsync(projectId);
            if (analytics == null)
                return NotFound();

            return Ok(analytics);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting analytics for project {ProjectId}", projectId);
            return StatusCode(500, "Failed to get project analytics");
        }
    }

    [HttpGet("project/summary")]
    public async Task<ActionResult<AnalyticsSummaryDto>> GetProjectAnalyticsSummary()
    {
        try
        {
            var summary = await analyticsService.GetProjectAnalyticsSummaryAsync();
            return Ok(summary);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting project analytics summary");
            return StatusCode(500, "Failed to get project analytics summary");
        }
    }

    [HttpGet("summary")]
    public async Task<ActionResult<CombinedAnalyticsSummaryDto>> GetCombinedAnalyticsSummary()
    {
        try
        {
            var summary = await analyticsService.GetCombinedAnalyticsSummaryAsync();
            return Ok(summary);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting combined analytics summary");
            return StatusCode(500, "Failed to get combined analytics summary");
        }
    }

    [HttpGet("stats")]
    public async Task<ActionResult> GetSystemStatistics()
    {
        var stats = new
        {
            CertificatesCount = await context.Certificates.CountAsync(),
            ProjectsCount = await context.Projects.CountAsync(),
            TechnologiesCount = await context.Technologies.CountAsync(),
            StacksCount = await context.Stacks.CountAsync(),
            SkillsCount = await context.Skills.CountAsync(),
            UsersCount = await context.Users.CountAsync(),
        };

        return Ok(stats);
    }
}