using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess.Interfaces;

public interface IListItemDataProvider
{
    /// <summary>
    /// Archive a listitem
    /// </summary>
    /// <param name="listId">Id of the list in which the listitem should be</param>
    /// <param name="listItemId">Id of the listitem</param>
    /// <param name="userId">Id of the user which archived the listitem</param>
    void Archive(int listId, int listItemId, int userId);
    /// <summary>
    /// Check or uncheck a listitem by userId
    /// </summary>
    /// <param name="listId">Id of the list</param>
    /// <param name="listItemId">Id of the listitem which is beeing checked or unchecked</param>
    /// <param name="userId">Id of the user</param>
    /// <param name="isChecked">Bool if the item is checked or unchecked</param>
    void Check(int listId, int listItemId, int userId, bool isChecked);
    /// <summary>
    /// Insert a new listitem in a list
    /// </summary>
    /// <param name="listItem">The new listitem</param>
    /// <param name="userId">Id of the user which added the listitem</param>
    /// <returns>Id of the new listitem</returns>
    int Insert(ListItem listItem, int userId);
    /// <summary>
    /// Update a listitem
    /// </summary>
    /// <param name="listItem">The listitem which is beeing updated</param>
    /// <param name="userId">Id of the user which is updating the listitem</param>
    void Update(ListItem listItem, int userId);
}
