using Microsoft.AspNetCore.Mvc;
using Pyco.Todo.Core.Authorization;
using Pyco.Todo.Core.Authorization.Attributes;
using Pyco.Todo.Data.Jwt;
using Pyco.Todo.Data.ViewModels;

namespace Pyco.Todo.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/[controller]")]
public class AuthenticationController : Controller
{
    private readonly IAuthenticationService _authenticationService;
    private readonly JwtOptions _jwtOptions;

    public AuthenticationController(
        IAuthenticationService authenticationService, 
        JwtOptions jwtOptions)
    {
        _authenticationService = authenticationService;
        _jwtOptions = jwtOptions;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Authenticate(AuthenticateRequest model)
    {
        AuthenticateResponse response = _authenticationService.Authenticate(model);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshTokenExpirationMinutes)
        };
        Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public IActionResult RefreshToken()
    {
        string? refreshToken = Request.Cookies["refreshToken"];
        AuthenticateResponse response = _authenticationService.RefreshToken(refreshToken ?? string.Empty);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshTokenExpirationMinutes)
        };
        Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);

        return Ok(response);
    }

    [HttpPost("revoke")]
    public IActionResult RevokeToken(string? token)
    {
        // accept refresh token in request body or cookie
        token ??= Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
        {
            return BadRequest(new { message = "Token is required" });
        }

        _authenticationService.RevokeToken(token);
        return Ok(new { message = "Token revoked" });
    }
}
