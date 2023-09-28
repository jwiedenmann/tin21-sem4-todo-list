using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Enumerable of all users</returns>
        IEnumerable<User> Get();
        /// <summary>
        /// Get user by its username
        /// </summary>
        /// <param name="username">username of the wanted user</param>
        /// <returns>The corrosponding user</returns>
        User? Get(string username);
        /// <summary>
        /// Returns the users within one list, including the useres roles.
        /// </summary>
        IEnumerable<User> GetListUsers(int listId);
        /// <summary>
        /// Get user by its refreshtoken
        /// </summary>
        /// <param name="token">The refreshtoken of the user</param>
        /// <returns>The corrosponding user</returns>
        User? GetByToken(string token);
        /// <summary>
        /// Checks whether a username already exists
        /// </summary>
        /// <param name="username">The username which has to be checked</param>
        /// <returns>Bool which is true if the username exists</returns>
        bool UsernameExists(string username);
        /// <summary>
        /// Gets the users by their username
        /// </summary>
        /// <param name="searchTerm">Input of searchbar</param>
        /// <param name="showArchived">True if archived items are needed</param>
        /// <returns>Returns the users which have the term in their username</returns>
        IEnumerable<User> Search(string searchTerm, bool showArchived = false);
        /// <summary>
        /// Inserts the user data into the database.
        /// </summary>
        /// <returns>The id of the created user.</returns>
        int? Insert(User user);
    }
}