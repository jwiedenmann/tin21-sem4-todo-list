using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAsync();
        Task<User?> GetAsync(string username);
        Task<User?> GetByTokenAsync(string token);
    }
}