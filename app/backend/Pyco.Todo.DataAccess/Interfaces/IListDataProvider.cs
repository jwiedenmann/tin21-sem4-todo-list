using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess.Interfaces;

public interface IListDataProvider
{
    /// <summary>
    /// Get the total list with listitems by userId
    /// </summary>
    /// <param name="listId">Id of the List</param>
    /// <param name="userId">Id of the user</param>
    /// <returns>Total list with listitems</returns>
    List? Get(int listId, int userId);
    /// <summary>
    /// Insert a new list wit hall the listusers
    /// </summary>
    /// <param name="list">The list which is beeing added</param>
    /// <returns>Returns the new listId</returns>
    int Insert(List list);
    /// <summary>
    /// Update a list
    /// </summary>
    /// <param name="list">The list which is beeing updated</param>
    /// <param name="userId">Id of the User</param>
    void Update(List list, int userId);
}
