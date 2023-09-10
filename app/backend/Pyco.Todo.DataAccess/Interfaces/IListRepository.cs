using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess.Interfaces
{
    public interface IListRepository
    {
        bool IsListUser(int listId, int userId);
        List? Get(int listId, bool showArchived = false);
        IEnumerable<List> GetByUser(int userId, bool archived = false);
    }
}