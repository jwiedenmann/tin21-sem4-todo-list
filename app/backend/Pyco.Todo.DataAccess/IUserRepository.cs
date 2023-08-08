using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAsync();
        Task<User?> GetAsync(string username);
        Task<User?> GetByTokenAsync(string token);
        Task<bool> UsernameExistsAsync(string username);
        /// <summary>
        /// Inserts the user data into the database.
        /// </summary>
        /// <returns>The id of the created user.</returns>
        Task<int?> InsertAsync(User user);
    }
}