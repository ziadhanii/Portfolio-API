using Portfolio.Contracts.Authentication;
using Portfolio.Contracts.Interfaces;

namespace Portfolio.Controllers;

public class AuthenticationController(IAuthService authService) : BaseApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var authResult = await authService.GenerateTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
    }
}