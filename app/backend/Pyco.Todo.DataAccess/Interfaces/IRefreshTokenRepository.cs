﻿using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess.Interfaces
{
    public interface IRefreshTokenRepository
    {
        RefreshToken? Get(int userId);
        int Set(RefreshToken refreshToken);
        int Delete(string token);
    }
}