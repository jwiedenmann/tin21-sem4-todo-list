using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetAsync(int userId);
        Task<int> SetAsync(RefreshToken refreshToken);
        Task<int> DeleteAsync(string token);
    }
}