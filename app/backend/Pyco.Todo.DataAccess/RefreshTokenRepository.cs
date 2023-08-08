using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly string _connectionstring;

    public RefreshTokenRepository(IConfiguration configuration)
    {
        _connectionstring = configuration.GetConnectionString("PycoDb");
    }

    public Task<RefreshToken?> GetAsync(int userId)
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
        return connection.QueryFirstOrDefaultAsync<RefreshToken?>(query, new { userId });
    }

    public Task<int> SetAsync(RefreshToken refreshToken)
    {
        const string query = @"
delete from refreshToken
where userId = @userId;

insert into refreshToken (userId, token, expires, created)
values (@userId, @token, @expires, @created);";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.ExecuteAsync(query, refreshToken);
    }

    public Task<int> DeleteAsync(string token)
    {
        const string query = @"
delete from refreshToken
where token = @token;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.ExecuteAsync(query, new { token });
    }
}
