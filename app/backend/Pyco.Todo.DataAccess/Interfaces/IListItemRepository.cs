using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess.Interfaces;

public interface IListItemRepository
{
    /// <summary>
    /// Archive a specific listitem(Task)
    /// </summary>
    /// <param name="id">Id of the corrosponding listitem</param>
    /// <returns>Returns the number of rows which where effected</returns>
    int Archive(int id);
    /// <summary>
    /// Check a listitem and add userId whom checked the item
    /// </summary>
    /// <param name="listItemId">Id of the listitem which got checked</param>
    /// <param name="userId">UserId of the user whom checked the listitem</param>
    /// <returns>Returns the number of rows which where effected</returns>
    int Check(int listItemId, int userId);
    /// <summary>
    /// Get all the listitems in corrosponding list
    /// </summary>
    /// <param name="listId">Id of the corrosponding list</param>
    /// <param name="showArchived">True if archived items are needed</param>
    /// <returns>List of ListItem</returns>
    IEnumerable<ListItem> Get(int listId, bool showArchived = false);
    /// <summary>
    /// Get a list of useres which checked a task
    /// </summary>
    /// <param name="listId">Id of the List</param>
    /// <returns>A dictionary with the listItemId and the users which checked the listitem</returns>
    Dictionary<int, List<int>> GetListItemChecks(int listId);
    /// <summary>
    /// Instert a new listitem into database
    /// </summary>
    /// <param name="list">A listitem to add to database</param>
    /// <returns>Returns the Id of the added listitem in the datatable</returns>
    int Insert(ListItem list);
    /// <summary>
    /// Uncheck a listitem and add userId whom unchecked the item
    /// </summary>
    /// <param name="listItemId">Id of the listitem which got unchecked</param>
    /// <param name="userId">UserId of the user whom unchecked the listitem</param>
    /// <returns>Returns the number of rows which where effected</returns>
    int Uncheck(int listItemId, int userId);
    /// <summary>
    /// Update a specific listitem
    /// </summary>
    /// <param name="list">Listitem whick is beeing updated</param>
    /// <returns>Returns the number of rows which where effected</returns>
    int Update(ListItem list);
}
