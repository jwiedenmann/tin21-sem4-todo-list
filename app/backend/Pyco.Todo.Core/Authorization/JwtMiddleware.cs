using Microsoft.AspNetCore.Http;
using Pyco.Todo.DataAccess;
using System.Security.Claims;

namespace Pyco.Todo.Core.Authorization;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserRepository userRepository, IJwtUtils jwtUtils)
    {
        string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        IEnumerable<Claim>? claims = jwtUtils.ValidateJwtToken(token);
        string? username = claims?.FirstOrDefault(x => x.Type == "username")?.Value;

        if (claims != null && username != null)
        {
            context.Items["User"] = userRepository.GetAsync(username);
        }

        await _next(context);
    }
}