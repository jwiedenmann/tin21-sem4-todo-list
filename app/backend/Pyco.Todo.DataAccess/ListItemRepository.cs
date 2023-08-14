using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess;

public class ListItemRepository
{
    private readonly string _connectionstring;

    public ListItemRepository(IConfiguration configuration)
    {
        _connectionstring = configuration.GetConnectionString("TodoDb");
    }

    public IEnumerable<ListItem> Get(int userId, bool archived = false)
    {
        const string query = @"
select
    l.id,
    l.title,
    l.archive,
    l.creationDate
from list as l
inner join listuser as lu on lu.listId = l.id
inner join ""user"" as u on u.id = lu.userId
where u.id = @userId and l.archive = @archived;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.Query<ListItem>(query, new { userId, archived });
    }
}
