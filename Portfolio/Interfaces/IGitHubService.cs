namespace Portfolio.Interfaces;

public interface IGitHubService
{
    Task<GitHubUserDto> GetUserAsync();
    Task<List<GitHubRepoDto>> GetReposAsync(int page = 1, int perPage = 30);
    Task<FeaturedProjectsDto> GetFeaturedProjectsAsync();
    Task<List<GitHubRepoDto>> GetPrivateReposAsync();
}