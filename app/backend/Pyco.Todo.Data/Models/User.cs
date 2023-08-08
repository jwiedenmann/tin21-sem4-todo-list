using System.Text.Json.Serialization;

namespace Pyco.Todo.Data.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    [JsonIgnore]
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Archive { get; set; }
    public DateTime CreationDate { get; set; }

    [JsonIgnore]
    public RefreshToken? RefreshToken { get; set; }
}
