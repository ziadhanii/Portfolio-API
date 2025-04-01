using Microsoft.AspNetCore.Authorization;
using Portfolio.DTOs;
using Portfolio.Services;

namespace Portfolio.Controllers;

[Authorize]
public class ProfileController(UserManager<ApplicationUser> userManager, IPictureService pictureService)
    : BaseApiController
{

    [HttpGet("")]
    public async Task<ActionResult<UserProfileDto>> GetProfile()
    {
        
        var user = await userManager.Users.FirstOrDefaultAsync();

        if (user == null)
            return NotFound();

        var profileDto = new UserProfileDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            ShortBio = user.ShortBio,
            Bio = user.Bio,
            ProfileImageUrl = user.ProfileImageUrl,
            JobTitle = user.JobTitle,
            Location = user.Location,
            LinkedInUrl = user.LinkedInUrl,
            GitHubUrl = user.GitHubUrl,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };

        return Ok(profileDto);
    }

    [HttpPut("")]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserProfileDto profileDto)
    {
        
        var user = await userManager.Users.FirstOrDefaultAsync();

        if (user == null)
            return NotFound();


        user.FirstName = profileDto.FirstName ?? user.FirstName;
        user.LastName = profileDto.LastName ?? user.LastName;
        user.ShortBio = profileDto.ShortBio ?? user.ShortBio;
        user.Bio = profileDto.Bio ?? user.Bio;
        user.JobTitle = profileDto.JobTitle ?? user.JobTitle;
        user.Location = profileDto.Location ?? user.Location;
        user.LinkedInUrl = profileDto.LinkedInUrl ?? user.LinkedInUrl;
        user.GitHubUrl = profileDto.GitHubUrl ?? user.GitHubUrl;
        user.PhoneNumber = profileDto.PhoneNumber ?? user.PhoneNumber;


        if (profileDto.ProfileImage != null)
        {
            var imageUrl =
                await pictureService.UpdatePictureAsync(profileDto.ProfileImage, user.ProfileImageUrl, Request);
            if (!string.IsNullOrEmpty(imageUrl))
            {
                user.ProfileImageUrl = imageUrl;
            }
        }


        await userManager.UpdateAsync(user);


        return NoContent();
    }
}