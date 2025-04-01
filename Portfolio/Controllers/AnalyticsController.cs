using Microsoft.AspNetCore.Mvc;
using Portfolio.DTOs;
using Portfolio.Services;

namespace Portfolio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly ILogger<AnalyticsController> _logger;

    public AnalyticsController(
        IAnalyticsService analyticsService,
        ILogger<AnalyticsController> logger)
    {
        _analyticsService = analyticsService;
        _logger = logger;
    }

    /// <summary>
    /// Track a project view
    /// </summary>
    [HttpPost("track-view/{projectId:int}")]
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

    /// <summary>
    /// Get analytics for a specific project
    /// </summary>
    [HttpGet("project/{projectId:int}")]
    [ProducesResponseType(typeof(ProjectAnalyticsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProjectAnalytics(int projectId)
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

    /// <summary>
    /// Get analytics summary for all projects
    /// </summary>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(AnalyticsSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAnalyticsSummary()
    {
        try
        {
            var summary = await _analyticsService.GetAnalyticsSummaryAsync();
            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting analytics summary");
            return StatusCode(500, "Failed to get analytics summary");
        }
    }
} 