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
        _connectionstring = configuration.GetConnectionString("PycoDb");
    }

    public Task<User> Get(string username)
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
where username = @username";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.QueryFirstAsync<User>(query, new { username });
    }
}
