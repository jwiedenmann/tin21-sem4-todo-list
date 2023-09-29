using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess.Interfaces
{
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Get refreshtoken by userId
        /// </summary>
        /// <param name="userId">Id of the corrosponding user</param>
        /// <returns>Returns the refreshtoken</returns>
        RefreshToken? Get(int userId);
        /// <summary>
        /// Sets a new refreshtoken for a user and deletes the old one by the userId
        /// </summary>
        /// <param name="refreshToken">New refreshtoken</param>
        /// <returns>Returns number of effected rows</returns>
        int Set(RefreshToken refreshToken);
        /// <summary>
        /// Delete a refreshtoken by the token
        /// </summary>
        /// <param name="token">The token which has to be deleted</param>
        /// <returns>Returns number of effected rows</returns>
        int Delete(string token);
    }
}