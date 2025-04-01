namespace Portfolio.Persistence;

public static class ApplicationDbSeeder
{
    public static async Task SeedAdmin(UserManager<ApplicationUser> userManager)
    {
        if (!await userManager.Users.AnyAsync())
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Ziad",
                LastName = "Hany",
                ShortBio = "Software Developer",
                Bio = "Passionate .NET Developer with experience in web applications.",
                ProfileImageUrl = "https://example.com/profile.jpg",
                JobTitle = "Backend Developer",
                Location = "Tanta, Egypt",
                LinkedInUrl = "https://linkedin.com/in/ziad-hani",
                GitHubUrl = "https://github.com/ziadhanii",
                DateCreated = DateTime.UtcNow,
                UserName = "ziadhany",
                NormalizedUserName = "ZIADHANY",
                Email = "ziadhani64@gmail.com",
                NormalizedEmail = "ZIADHANY64@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "01554530991",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0
            };

            const string password = "Zoz332003##";
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                Console.WriteLine("User created successfully!");
            }
            else
            {
                Console.WriteLine("Error: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}