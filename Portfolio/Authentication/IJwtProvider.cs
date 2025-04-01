namespace Portfolio.Authentication;

public interface IJwtProvider
{
    string GenerateToken(ApplicationUser user);
}