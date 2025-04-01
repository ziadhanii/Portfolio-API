using Microsoft.AspNetCore.Mvc;
using Portfolio.DTOs;
using Portfolio.Services;

namespace Portfolio.Controllers;


public class GitHubController : BaseApiController
{
    private readonly IGitHubService _gitHubService;
    private readonly ILogger<GitHubController> _logger;

    public GitHubController(
        IGitHubService gitHubService,
        ILogger<GitHubController> logger)
    {
        _gitHubService = gitHubService;
        _logger = logger;
    }

    /// <summary>
    /// Get GitHub user profile information
    /// </summary>
    [HttpGet("profile")]
    [ProducesResponseType(typeof(GitHubUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetGitHubProfile()
    {
        try
        {
            var profile = await _gitHubService.GetUserAsync();
            return Ok(profile);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching GitHub profile");
            return StatusCode(500, "Failed to fetch GitHub profile");
        }
    }

    /// <summary>
    /// Get featured GitHub projects
    /// </summary>
    [HttpGet("projects")]
    [ProducesResponseType(typeof(FeaturedProjectsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFeaturedProjects()
    {
        try
        {
            var projects = await _gitHubService.GetFeaturedProjectsAsync();
            return Ok(projects);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching featured projects");
            return StatusCode(500, "Failed to fetch featured projects");
        }
    }

    /// <summary>
    /// Get private GitHub repositories
    /// </summary>
    [HttpGet("private-repos")]
    [ProducesResponseType(typeof(List<GitHubRepoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPrivateRepos()
    {
        try
        {
            var repos = await _gitHubService.GetPrivateReposAsync();
            return Ok(repos);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching private repositories");
            return StatusCode(500, "Failed to fetch private repositories");
        }
    }

    [HttpGet("repos")]
    [ProducesResponseType(typeof(List<GitHubRepoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGitHubRepos([FromQuery] int page = 1, [FromQuery] int perPage = 30)
    {
        try
        {
            var repos = await _gitHubService.GetReposAsync(page, perPage);
            return Ok(repos);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching GitHub repositories");
            return StatusCode(500, "Failed to fetch GitHub repositories");
        }
    }

    [HttpGet("repos/{owner}/{repoName}")]
    [ProducesResponseType(typeof(GitHubRepoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGitHubRepo(string owner, string repoName)
    {
        try
        {
            var repo = await _gitHubService.GetRepoAsync(owner, repoName);
            return Ok(repo);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching GitHub repository {Owner}/{RepoName}", owner, repoName);
            return StatusCode(500, "Failed to fetch GitHub repository");
        }
    }
}

public class GitHubUserDto
{
    public string Login { get; set; }
    public int Id { get; set; }
    public string AvatarUrl { get; set; }
    public string Url { get; set; }
    public string HtmlUrl { get; set; }
    public string FollowersUrl { get; set; }
    public string FollowingUrl { get; set; }
    public string GistsUrl { get; set; }
    public string StarredUrl { get; set; }
    public string SubscriptionsUrl { get; set; }
    public string OrganizationsUrl { get; set; }
    public string ReposUrl { get; set; }
    public string EventsUrl { get; set; }
    public string ReceivedEventsUrl { get; set; }
    public string Type { get; set; }
    public bool SiteAdmin { get; set; }
    public string Name { get; set; }
    public string Company { get; set; }
    public string Blog { get; set; }
    public string Location { get; set; }
    public string Email { get; set; }
    public bool? Hireable { get; set; }
    public string Bio { get; set; }
    public string TwitterUsername { get; set; }
    public int PublicRepos { get; set; }
    public int PublicGists { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int PrivateGists { get; set; }
    public int TotalPrivateRepos { get; set; }
    public int OwnedPrivateRepos { get; set; }
    public int DiskUsage { get; set; }
    public int Collaborators { get; set; }
    public bool TwoFactorAuthentication { get; set; }
    public GitHubPlanDto Plan { get; set; }
}

public class GitHubPlanDto
{
    public string Name { get; set; }
    public long Space { get; set; }
    public int Collaborators { get; set; }
    public int PrivateRepos { get; set; }
}

public class GitHubRepoDto
{
    public int id { get; set; }
    public string name { get; set; }
    public bool @private { get; set; }
    public object description { get; set; }
    public string url { get; set; }
}