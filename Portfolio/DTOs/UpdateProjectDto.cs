namespace Portfolio.DTOs;

public class UpdateProjectDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
    public string ProjectUrl { get; set; } = string.Empty;
    public string GitHubRepo { get; set; } = string.Empty;
}