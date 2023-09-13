using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Pyco.Todo.Data.Models;
using Pyco.Todo.DataAccess.Interfaces;

namespace Pyco.Todo.DataAccess.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly string _connectionstring;

    public RefreshTokenRepository(IConfiguration configuration)
    {
        _connectionstring = configuration.GetConnectionString("TodoDb");
    }

    public RefreshToken? Get(int userId)
    {
        const string query = @"
select
    userId,
    token,
    expires,
    created
from refreshToken
where userId = @userId;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.QueryFirstOrDefault<RefreshToken?>(query, new { userId });
    }

    public int Set(RefreshToken refreshToken)
    {
        const string query = @"
delete from refreshToken
where userId = @userId;

insert into refreshToken (userId, token, expires, created)
values (@userId, @token, @expires, @created);";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.Execute(query, refreshToken);
    }

    public int Delete(string token)
    {
        const string query = @"
delete from refreshToken
where token = @token;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.Execute(query, new { token });
    }
}
