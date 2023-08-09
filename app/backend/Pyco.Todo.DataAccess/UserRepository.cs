using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess;

public class UserRepository : IUserRepository
{
    private readonly string _connectionstring;

    public UserRepository(IConfiguration configuration)
    {
        _connectionstring = configuration.GetConnectionString("TodoDb");
    }

    public IEnumerable<User> Get()
    {
        const string query = @"
select
    id,
    username,
    password,
    email,
    archive,
    creationDate
from ""user"";";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.Query<User>(query);
    }

    public User? Get(string username)
    {
        const string query = @"
select
    id,
    username,
    password,
    email,
    archive,
    creationDate
from ""user""
where username = @username;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.QueryFirstOrDefault<User?>(query, new { username });
    }

    public User? GetByToken(string token)
    {
        const string query = @"
select
    u.id,
    u.username,
    u.password,
    u.email,
    u.archive,
    u.creationDate
from ""user"" as u
inner join refreshToken as r on r.userId = u.id
where r.token = @token;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.QueryFirstOrDefault<User?>(query, new { token });
    }

    public bool UsernameExists(string username)
    {
        const string query = @"
select 1
from ""user""
where username = @username;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return (connection.QueryFirstOrDefault<int>(query, new { username })) != 0;
    }

    public int? Insert(User user)
    {
        const string query = @"
insert into ""user"" (username, password, email)
values (@username, @password, @email)
on conflict (username) do nothing
returning id;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.QueryFirstOrDefault<int?>(query, user);
    }
}
