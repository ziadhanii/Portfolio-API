namespace Portfolio.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";
    public string ShortBio { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string ProfileImageUrl { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string LinkedInUrl { get; set; } = string.Empty;
    public string GitHubUrl { get; set; } = string.Empty;
    
    public string CvUrl { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }

}
