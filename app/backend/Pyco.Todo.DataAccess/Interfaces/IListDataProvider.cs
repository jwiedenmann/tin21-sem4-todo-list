using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess.Interfaces;

public interface IListDataProvider
{
    List? Get(int listId, int userId);
}
