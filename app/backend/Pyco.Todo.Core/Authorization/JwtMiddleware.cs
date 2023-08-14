using Microsoft.AspNetCore.Http;
using Pyco.Todo.DataAccess.Interfaces;
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
        token ??= context.Request.Cookies["jwt"];
        IEnumerable<Claim>? claims = jwtUtils.ValidateJwtToken(token);
        string? username = claims?.FirstOrDefault(x => x.Type == "username")?.Value;

        if (claims != null && username != null)
        {
            context.Items["User"] = userRepository.Get(username);
        }

        await _next(context);
    }
}