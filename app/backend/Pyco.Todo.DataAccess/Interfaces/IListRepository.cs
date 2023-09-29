using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess.Interfaces
{
    public interface IListRepository
    {
        /// <summary>
        /// Checks whether a user ist part of a list
        /// </summary>
        /// <param name="listId">Id of the List</param>
        /// <param name="userId">Id of the user</param>
        /// <returns>Returns bool which says if user ist part of list or not</returns>
        bool IsListUser(int listId, int userId);
        /// <summary>
        /// Checks whether a user is a admin of a specific list
        /// </summary>
        /// <param name="listId">Id of the List</param>
        /// <param name="userId">Id of the user</param>
        /// <returns>Returns bool which says if user ist part of list and admin of the list or not</returns>
        bool IsListAdmin(int listId, int userId);
        /// <summary>
        /// Get a list by its Id
        /// </summary>
        /// <param name="listId">Id of the List</param>
        /// <param name="showArchived">Bool if archived lists are needed</param>
        /// <returns>Returns a Todo-List</returns>
        List? Get(int listId, bool showArchived = false);
        /// <summary>
        /// Get all the lists of a user by his Id
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="showArchived">Bool if archived lists are needed</param>
        /// <returns>Returns a enumerable of lists</returns>
        IEnumerable<List> GetByUser(int userId, bool archived = false);
        /// <summary>
        /// Insert a new list
        /// </summary>
        /// <param name="list">The list which is beeing added</param>
        /// <returns>The new Id of the added list</returns>
        int Insert(List list);
        /// <summary>
        /// Insert a new user into a list.
        /// If the user is already in list, he won`t be added
        /// </summary>
        /// <param name="listId">Id of the list</param>
        /// <param name="userId">Id of the user</param>
        /// <param name="userRole">Role of the new user</param>
        /// <returns></returns>
        int InsertListUser(int listId, int userId, Role userRole);
        /// <summary>
        /// Update the title of a list
        /// </summary>
        /// <param name="list">The list whos title is beeing updated</param>
        /// <returns></returns>
        int Update(List list);
        /// <summary>
        /// Delete all listuser by the listId
        /// </summary>
        /// <param name="listId">Id of the list which is beeing deleted</param>
        /// <returns>Returns number of effected rows</returns>
        int DeleteListUsers(int listId);
    }
}