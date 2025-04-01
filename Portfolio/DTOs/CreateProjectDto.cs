namespace Portfolio.DTOs;

public class CreateProjectDto
{
    [Required, MaxLength(200)] public required string Title { get; set; }

    [Required, MaxLength(1000)] public required string Description { get; set; }

    [Required] public required IFormFile Image { get; set; }

    [Required, Url] public required string ProjectUrl { get; set; }

    [Url] public required string GitHubRepo { get; set; }

    public List<int> TechnologyIds { get; set; } = new();
    
}