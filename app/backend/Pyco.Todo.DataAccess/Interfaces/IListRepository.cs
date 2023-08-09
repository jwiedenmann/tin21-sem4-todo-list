using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess.Interfaces
{
    public interface IListRepository
    {
        IEnumerable<List> Get(int userId, bool archived = false);
    }
}