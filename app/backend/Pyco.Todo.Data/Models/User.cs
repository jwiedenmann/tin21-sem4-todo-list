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

    // realization of a property that is never serialized to json, but always deserialized
#pragma warning disable IDE0051 // Remove unused private members
    [JsonPropertyName(nameof(Password))]
    public string PasswordSetter
    {
        set => Password = value;
    }
#pragma warning restore IDE0051 // Remove unused private members

    [JsonIgnore]
    public RefreshToken? RefreshToken { get; set; }

    // the role of the user within one specific list, e.g listadmin
    public Role ListUserRole { get; set; }
}
