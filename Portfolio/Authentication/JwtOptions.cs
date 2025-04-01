namespace Portfolio.Authentication;

public class JwtOptions
{
    public static string SectionName = "Jwt";

    [Required] public string Key { get; init; } = string.Empty;
}