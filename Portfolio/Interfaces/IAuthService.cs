using Portfolio.Contracts.Authentication;

namespace Portfolio.Contracts.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponse>> GenerateTokenAsync(string email, string password,
        CancellationToken cancellationToken = default);
}