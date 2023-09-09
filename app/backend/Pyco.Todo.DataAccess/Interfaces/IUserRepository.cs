﻿using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> Get();
        User? Get(string username);
        User? GetByToken(string token);
        bool UsernameExists(string username);
        IEnumerable<User> Search(string searchTerm);
        /// <summary>
        /// Inserts the user data into the database.
        /// </summary>
        /// <returns>The id of the created user.</returns>
        int? Insert(User user);
    }
}