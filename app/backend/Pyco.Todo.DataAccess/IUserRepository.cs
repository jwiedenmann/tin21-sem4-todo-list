using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess
{
    public interface IUserRepository
    {
        Task<User> Get(string username);
    }
}