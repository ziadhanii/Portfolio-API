using Microsoft.AspNetCore.Mvc;
using Portfolio.DTOs;
using Portfolio.Services;

namespace Portfolio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UnifiedAnalyticsController : ControllerBase
{
    private readonly IUnifiedAnalyticsService _analyticsService;
    private readonly ILogger<UnifiedAnalyticsController> _logger;

    public UnifiedAnalyticsController(
        IUnifiedAnalyticsService analyticsService,
        ILogger<UnifiedAnalyticsController> logger)
    {
        _analyticsService = analyticsService;
        _logger = logger;
    }

    // Website Analytics Endpoints
    [HttpPost("website/track-view")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> TrackPageView([FromBody] TrackPageViewRequest request)
    {
        try
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers["User-Agent"].ToString();
            var referrer = Request.Headers["Referer"].ToString();

            await _analyticsService.TrackPageViewAsync(
                request.PagePath,
                request.PageTitle,
                ipAddress,
                userAgent,
                referrer);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking page view");
            return StatusCode(500, "Error tracking page view");
        }
    }

    [HttpGet("website/page/{pagePath}")]
    [ProducesResponseType(typeof(WebsiteAnalyticsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WebsiteAnalyticsDto>> GetPageAnalytics(string pagePath)
    {
        try
        {
            var analytics = await _analyticsService.GetPageAnalyticsAsync(pagePath);
            if (analytics == null)
                return NotFound();

            return Ok(analytics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting page analytics for {PagePath}", pagePath);
            return StatusCode(500, "Error getting page analytics");
        }
    }

    [HttpGet("website/summary")]
    [ProducesResponseType(typeof(WebsiteAnalyticsSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WebsiteAnalyticsSummaryDto>> GetWebsiteSummary()
    {
        try
        {
            var summary = await _analyticsService.GetWebsiteSummaryAsync();
            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting website summary");
            return StatusCode(500, "Error getting website summary");
        }
    }

    // Project Analytics Endpoints
    [HttpPost("project/track-view/{projectId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> TrackProjectView(int projectId)
    {
        try
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers["User-Agent"].ToString();
            var referrer = Request.Headers["Referer"].ToString();

            await _analyticsService.TrackProjectViewAsync(projectId, ipAddress, userAgent, referrer);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking project view for project {ProjectId}", projectId);
            return StatusCode(500, "Failed to track project view");
        }
    }

    [HttpGet("project/{projectId:int}")]
    [ProducesResponseType(typeof(ProjectAnalyticsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProjectAnalyticsDto>> GetProjectAnalytics(int projectId)
    {
        try
        {
            var analytics = await _analyticsService.GetProjectAnalyticsAsync(projectId);
            if (analytics == null)
                return NotFound();

            return Ok(analytics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting analytics for project {ProjectId}", projectId);
            return StatusCode(500, "Failed to get project analytics");
        }
    }

    [HttpGet("project/summary")]
    [ProducesResponseType(typeof(AnalyticsSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AnalyticsSummaryDto>> GetProjectAnalyticsSummary()
    {
        try
        {
            var summary = await _analyticsService.GetProjectAnalyticsSummaryAsync();
            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting project analytics summary");
            return StatusCode(500, "Failed to get project analytics summary");
        }
    }

    // Combined Analytics Endpoint
    [HttpGet("summary")]
    [ProducesResponseType(typeof(CombinedAnalyticsSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CombinedAnalyticsSummaryDto>> GetCombinedAnalyticsSummary()
    {
        try
        {
            var summary = await _analyticsService.GetCombinedAnalyticsSummaryAsync();
            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting combined analytics summary");
            return StatusCode(500, "Failed to get combined analytics summary");
        }
    }
} 