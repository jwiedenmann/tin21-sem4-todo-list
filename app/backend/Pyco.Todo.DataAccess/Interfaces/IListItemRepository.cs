using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess.Interfaces;

public interface IListItemRepository
{
    IEnumerable<ListItem> Get(int listId, bool showArchived = false);
    /// <summary>
    /// Gets all the listitem ids and the users ids that checked these elements.
    /// </summary>
    /// <returns>A Dictionary with the ListItemId as the key and the User Ids as values.</returns>
    Dictionary<int, List<int>> GetListItemChecks(int listId);
}
