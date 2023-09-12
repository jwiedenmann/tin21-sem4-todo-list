using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Pyco.Todo.Data.Models;
using Pyco.Todo.DataAccess.Interfaces;

namespace Pyco.Todo.DataAccess.Repositories;

public class ListItemRepository : IListItemRepository
{
    private readonly string _connectionstring;

    public ListItemRepository(IConfiguration configuration)
    {
        _connectionstring = configuration.GetConnectionString("TodoDb");
    }

    public IEnumerable<ListItem> Get(int listId, bool showArchived = false)
    {
        const string query = @"
select
    li.id,
    li.listId,
    li.typeId,
    li.content,
    li.archive,
    li.creationDate
from listitem as li
where
    li.id = @listId and
    (li.archive = FALSE or li.archive = @showArchived);";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.Query<ListItem>(query, new { listId, showArchived });
    }
}
