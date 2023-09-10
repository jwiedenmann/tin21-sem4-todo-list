using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess.Interfaces;

public interface IListItemRepository
{
    IEnumerable<ListItem> Get(int listId, bool showArchived = false);
}
