namespace Portfolio.DTOs;

public class UserProfileDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string ShortBio { get; set; }
    public string Bio { get; set; }
    public string ProfileImageUrl { get; set; }
    public string JobTitle { get; set; }
    public string Location { get; set; }
    public string LinkedInUrl { get; set; }
    public string GitHubUrl { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    public string CvUrl { get; set; }

}

public class UpdateUserProfileDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ShortBio { get; set; }
    public string? Bio { get; set; }
    public IFormFile? ProfileImage { get; set; }
    public string? JobTitle { get; set; }
    public string? Location { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? PhoneNumber { get; set; }
    
    public string? CvUrl { get; set; } = string.Empty;

}