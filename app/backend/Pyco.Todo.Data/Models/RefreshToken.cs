namespace Pyco.Todo.Data.Models;

public class RefreshToken
{
    public int UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }

    public bool IsExpired => DateTime.UtcNow >= Expires;
}
