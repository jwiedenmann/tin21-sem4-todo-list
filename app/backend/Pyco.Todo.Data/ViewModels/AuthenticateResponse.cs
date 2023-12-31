using Pyco.Todo.Data.Models;
using System.Text.Json.Serialization;

namespace Pyco.Todo.Data.ViewModels;

public class AuthenticateResponse
{
    public AuthenticateResponse(User user, string jwtToken, string refreshToken)
    {
        Id = user.Id;
        Username = user.Username;
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }

    // neccessary for json deserialization
    public AuthenticateResponse() { }

    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string JwtToken { get; set; } = string.Empty;

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; } = string.Empty;
}