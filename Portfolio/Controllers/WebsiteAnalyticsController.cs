using Microsoft.AspNetCore.Mvc;
using Portfolio.DTOs;
using Portfolio.Services;

namespace Portfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebsiteAnalyticsController : ControllerBase
    {
        private readonly IWebsiteAnalyticsService _analyticsService;
        private readonly ILogger<WebsiteAnalyticsController> _logger;

        public WebsiteAnalyticsController(
            IWebsiteAnalyticsService analyticsService,
            ILogger<WebsiteAnalyticsController> logger)
        {
            _analyticsService = analyticsService;
            _logger = logger;
        }

        [HttpPost("track-view")]
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

        [HttpGet("page/{pagePath}")]
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

        [HttpGet("summary")]
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
    }

    public class TrackPageViewRequest
    {
        public string PagePath { get; set; }
        public string PageTitle { get; set; }
    }
} 