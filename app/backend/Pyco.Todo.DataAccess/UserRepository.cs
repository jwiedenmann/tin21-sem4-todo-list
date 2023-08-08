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

    public Task<IEnumerable<User>> GetAsync()
    {
        const string query = @"
select
    id,
    username,
    password,
    email,
    archive,
    creationDate
from user;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.QueryAsync<User>(query);
    }

    public Task<User?> GetAsync(string username)
    {
        const string query = @"
select
    id,
    username,
    password,
    email,
    archive,
    creationDate
from user
where username = @username;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.QueryFirstOrDefaultAsync<User?>(query, new { username });
    }

    public Task<User?> GetByTokenAsync(string token)
    {
        const string query = @"
select
    u.id,
    u.username,
    u.password,
    u.email,
    u.archive,
    u.creationDate
from user as u
inner join refreshToken as r on r.token = @token;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.QueryFirstOrDefaultAsync<User?>(query, new { token });
    }
}
