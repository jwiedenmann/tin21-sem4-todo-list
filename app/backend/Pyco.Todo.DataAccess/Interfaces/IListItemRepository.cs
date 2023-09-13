using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess.Interfaces;

public interface IListItemRepository
{
    int Archive(int id);
    int Check(int listItemId, int userId);
    IEnumerable<ListItem> Get(int listId, bool showArchived = false);
    /// <summary>
    /// Gets all the listitem ids and the users ids that checked these elements.
    /// </summary>
    /// <returns>A Dictionary with the ListItemId as the key and the User Ids as values.</returns>
    Dictionary<int, List<int>> GetListItemChecks(int listId);
    int Insert(ListItem list);
    int Uncheck(int listItemId, int userId);
    int Update(ListItem list);
}
