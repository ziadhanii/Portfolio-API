using System.Text.Json.Serialization;

namespace Portfolio.DTOs;

/// <summary>
/// Represents the GitHub user profile information
/// </summary>
public class GitHubUserDto
{
    [JsonPropertyName("login")]
    public string Login { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; }

    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; }

    [JsonPropertyName("bio")]
    public string Bio { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("public_repos")]
    public int PublicRepos { get; set; }

    [JsonPropertyName("private_repos")]
    public int PrivateRepos { get; set; }

    [JsonPropertyName("followers")]
    public int Followers { get; set; }

    [JsonPropertyName("following")]
    public int Following { get; set; }
}

/// <summary>
/// Represents a GitHub repository
/// </summary>
public class GitHubRepoDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; }

    [JsonPropertyName("stargazers_count")]
    public int StargazersCount { get; set; }

    [JsonPropertyName("forks_count")]
    public int ForksCount { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Represents featured GitHub projects categorized by different criteria
/// </summary>
public class FeaturedProjectsDto
{
    [JsonPropertyName("most_starred")]
    public List<GitHubRepoDto> MostStarred { get; set; }

    [JsonPropertyName("most_recent")]
    public List<GitHubRepoDto> MostRecent { get; set; }

    [JsonPropertyName("most_forked")]
    public List<GitHubRepoDto> MostForked { get; set; }
}

/// <summary>
/// Represents GitHub user statistics
/// </summary>
public class GitHubStatsDto
{
    [JsonPropertyName("total_repositories")]
    public int TotalRepositories { get; set; }

    [JsonPropertyName("total_stars")]
    public int TotalStars { get; set; }

    [JsonPropertyName("total_forks")]
    public int TotalForks { get; set; }

    [JsonPropertyName("total_followers")]
    public int TotalFollowers { get; set; }

    [JsonPropertyName("top_languages")]
    public List<string> TopLanguages { get; set; }

    [JsonPropertyName("language_stats")]
    public Dictionary<string, int> LanguageStats { get; set; }
}

/// <summary>
/// Represents GitHub user activity metrics
/// </summary>
public class GitHubActivityDto
{
    [JsonPropertyName("recent_repositories")]
    public List<GitHubRepoDto> RecentRepositories { get; set; }

    [JsonPropertyName("commits_last_month")]
    public int CommitsLastMonth { get; set; }

    [JsonPropertyName("pull_requests_last_month")]
    public int PullRequestsLastMonth { get; set; }

    [JsonPropertyName("issues_last_month")]
    public int IssuesLastMonth { get; set; }
} 