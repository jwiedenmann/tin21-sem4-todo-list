using Pyco.Todo.Data.Models;
using System.Security.Claims;

namespace Pyco.Todo.Core.Authorization;

public interface IJwtUtils
{
    public string GenerateJwtToken(User user);
    public IEnumerable<Claim>? ValidateJwtToken(string? token);
    public RefreshToken GenerateRefreshToken();
}
