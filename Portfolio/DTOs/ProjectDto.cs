namespace Portfolio.DTOs;

public class ProjectDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string ProjectUrl { get; set; }
    public string GitHubRepo { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<TechnologyResponseDto> Technologies { get; set; } = new List<TechnologyResponseDto>();
}