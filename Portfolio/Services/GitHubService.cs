using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Portfolio.DTOs;

namespace Portfolio.Services;

public class GitHubService : IGitHubService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<GitHubService> _logger;
    private const string GitHubApiBase = "https://api.github.com";

    public GitHubService(
        IHttpClientFactory httpClientFactory,
        IConfiguration config,
        IMemoryCache cache,
        ILogger<GitHubService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("GitHub");
        var gitHubToken = config["GitHub:Token"] ?? "";
        _cache = cache;
        _logger = logger;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", gitHubToken);
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
    }

    public async Task<GitHubUserDto> GetUserAsync()
    {
        try
        {
            var cacheKey = "github_user";
            if (_cache.TryGetValue(cacheKey, out GitHubUserDto cachedUser))
            {
                return cachedUser;
            }

            var response = await _httpClient.GetAsync($"{GitHubApiBase}/user");
            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<GitHubUserDto>();

            // Get private repos count
            var privateReposResponse = await _httpClient.GetAsync($"{GitHubApiBase}/user/repos?visibility=private");
            privateReposResponse.EnsureSuccessStatusCode();
            var privateRepos = await privateReposResponse.Content.ReadFromJsonAsync<List<GitHubRepoDto>>();
            user.PrivateRepos = privateRepos?.Count ?? 0;

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

            _cache.Set(cacheKey, user, cacheOptions);

            return user;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching GitHub user data");
            throw;
        }
    }

    public async Task<List<GitHubRepoDto>> GetPrivateReposAsync()
    {
        try
        {
            var cacheKey = "github_private_repos";
            if (_cache.TryGetValue(cacheKey, out List<GitHubRepoDto> cachedRepos))
            {
                return cachedRepos;
            }

            var response = await _httpClient.GetAsync(
                $"{GitHubApiBase}/user/repos?visibility=private&sort=updated&per_page=100");

            response.EnsureSuccessStatusCode();

            var repos = await response.Content.ReadFromJsonAsync<List<GitHubRepoDto>>();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

            _cache.Set(cacheKey, repos, cacheOptions);

            return repos;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching private GitHub repositories");
            throw;
        }
    }

    public async Task<List<GitHubRepoDto>> GetReposAsync(int page = 1, int perPage = 30)
    {
        try
        {
            var cacheKey = $"github_repos_page_{page}";
            if (_cache.TryGetValue(cacheKey, out List<GitHubRepoDto> cachedRepos))
            {
                return cachedRepos;
            }

            var response = await _httpClient.GetAsync(
                $"{GitHubApiBase}/user/repos?page={page}&per_page={perPage}&sort=updated&visibility=public");

            response.EnsureSuccessStatusCode();

            var repos = await response.Content.ReadFromJsonAsync<List<GitHubRepoDto>>();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

            _cache.Set(cacheKey, repos, cacheOptions);

            return repos;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching GitHub repositories");
            throw;
        }
    }

    public async Task<GitHubRepoDto> GetRepoAsync(string owner, string repoName)
    {
        try
        {
            var cacheKey = $"github_repo_{owner}_{repoName}";
            if (_cache.TryGetValue(cacheKey, out GitHubRepoDto cachedRepo))
            {
                return cachedRepo;
            }

            var response = await _httpClient.GetAsync($"{GitHubApiBase}/repos/{owner}/{repoName}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"Repository {owner}/{repoName} not found");
            }

            response.EnsureSuccessStatusCode();
            var repo = await response.Content.ReadFromJsonAsync<GitHubRepoDto>();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

            _cache.Set(cacheKey, repo, cacheOptions);

            return repo;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching GitHub repository {Owner}/{RepoName}", owner, repoName);
            throw;
        }
    }

    public async Task<FeaturedProjectsDto> GetFeaturedProjectsAsync()
    {
        try
        {
            var cacheKey = "github_featured_projects";
            if (_cache.TryGetValue(cacheKey, out FeaturedProjectsDto cachedProjects))
            {
                return cachedProjects;
            }

            var repos = await GetReposAsync(1, 100); // Get more repos to have better selection

            var featuredProjects = new FeaturedProjectsDto
            {
                MostStarred = repos
                    .OrderByDescending(r => r.StargazersCount)
                    .Take(5)
                    .ToList(),
                MostRecent = repos
                    .OrderByDescending(r => r.UpdatedAt)
                    .Take(5)
                    .ToList(),
                MostForked = repos
                    .OrderByDescending(r => r.ForksCount)
                    .Take(5)
                    .ToList()
            };

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));

            _cache.Set(cacheKey, featuredProjects, cacheOptions);

            return featuredProjects;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching featured projects");
            throw;
        }
    }
    
}