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
    li.listId = @listId and
    (li.archive = FALSE or li.archive = @showArchived);";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.Query<ListItem>(query, new { listId, showArchived });
    }

    public Dictionary<int, List<int>> GetListItemChecks(int listId)
    {
        const string query = @"
select
    lic.listItemId,
    lic.userId
from listitem as li
inner join listitemcheck as lic on lic.listItemId = li.id
where
    li.listId = @listId;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();

        Dictionary<int, List<int>> listItemCheckedUsers = new();
        connection.Query(
            sql: query,
            param: new { listId },
            types: new Type[] { typeof(int), typeof(int) },
            splitOn: "userId",
            map: (object[] objs) =>
            {
                if(objs.Length != 2
                || objs[0] is not int listItemId
                || objs[1] is not int userId)
                {
                    return 0;
                }

                if(!listItemCheckedUsers.TryGetValue(listItemId, out List<int>? userIds)) 
                {
                    userIds = new List<int>();
                    listItemCheckedUsers.Add(listItemId, userIds);
                }

                userIds.Add(userId);
                return 0;
            });

        return listItemCheckedUsers;
    }
}
