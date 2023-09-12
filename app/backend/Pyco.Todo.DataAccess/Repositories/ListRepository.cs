using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Pyco.Todo.Data.Models;
using Pyco.Todo.DataAccess.Interfaces;

namespace Pyco.Todo.DataAccess.Repositories;

public class ListRepository : IListRepository
{
    private readonly string _connectionstring;

    public ListRepository(IConfiguration configuration)
    {
        _connectionstring = configuration.GetConnectionString("TodoDb");
    }

    public bool IsListUser(int listId, int userId)
    {
        const string query = @"
select 1
from listuser as lu
where lu.listId = @listId and lu.userId = @userId;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.QueryFirstOrDefault<bool>(query, new { listId, userId });
    }

    public bool IsListAdmin(int listId, int userId)
    {
        const string query = @"
select 1
from listuser as lu
where lu.listId = @listId and lu.userId = @userId and lu.roleId = 1;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.QueryFirstOrDefault<bool>(query, new { listId, userId });
    }

    public List? Get(int listId, bool showArchived = false)
    {
        const string query = @"
select
    l.id,
    l.title,
    l.archive,
    l.creationDate
from list as l
where
    l.id = @listId and
    (l.archive = FALSE or l.archive = @showArchived);";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.QueryFirstOrDefault<List>(query, new { listId, showArchived });
    }

    public IEnumerable<List> GetByUser(int userId, bool showArchived = false)
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
where
    u.id = @userId and
    (l.archive = FALSE or l.archive = @showArchived);";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.Query<List>(query, new { userId, showArchived });
    }

    public int Insert(List list)
    {
        const string query = @"
insert into list (title)
values (@title)
returning id;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.QueryFirstOrDefault<int>(query, list);
    }

    public int InsertListUser(int listId, int userId, Role userRole)
    {
        const string query = @"
insert into listuser (listId, userId, roleId)
values (@listId, @userId, @userRole)
on conflict (listId, userId) do nothing;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.Execute(query, new { listId, userId, userRole });
    }

    public int Update(List list)
    {
        const string query = @"
update list
set titel = @title
where id = @id;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.Execute(query, list);
    }

    public int DeleteListUsers(int listId)
    {
        const string query = @"
delete from listuser
where listId = @listId;";

        using var connection = new NpgsqlConnection(_connectionstring);
        connection.Open();
        return connection.Execute(query, new { listId });
    }
}
