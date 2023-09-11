using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Pyco.Todo.Data.Models;
using Pyco.Todo.DataAccess.Interfaces;

namespace Pyco.Todo.DataAccess.Repositories;

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

    public IEnumerable<User> GetListUsers(int listId)
    {
        const string query = @"
select
    u.id,
    u.username,
    u.email,
    u.archive,
    u.creationDate,
    lu.roleId as ListUserRole
from ""user"" as u
inner join listuser as lu on lu.userId = u.id and lu.listId = @listId";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.Query<User>(query, new { listId });
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
where r.token = @token and u.archive = 0;";

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
        return connection.QueryFirstOrDefault<int>(query, new { username }) != 0;
    }

    public IEnumerable<User> Search(string searchTerm, bool showArchived = false)
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
where
    lower(username) like lower(@searchTerm) and
    (archive = 0 or archive = @showArchived);";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.Query<User>(query, new { searchTerm, showArchived });
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
