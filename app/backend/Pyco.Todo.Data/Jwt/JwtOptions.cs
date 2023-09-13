namespace Pyco.Todo.Data.Jwt;

public class JwtOptions
{
    public string Secret { get; set; } = string.Empty;
    public int JwtExpirationMinutes { get; set; }
    public int RefreshTokenExpirationMinutes { get; set; }
}
