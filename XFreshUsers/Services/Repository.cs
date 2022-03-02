using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFreshUsers.Models;

namespace XFreshUsers.Services
{
    public class Repository
    {
        private readonly SQLiteAsyncConnection conn;

        public string StatusMessage { get; set; }

        public Repository(string dbPath)
        {
            conn = new SQLiteAsyncConnection(dbPath);
            conn.CreateTableAsync<Models.User>().Wait();
        }

        public async Task CreateUser(User user)
        {
            try
            {
                // Basic validation to ensure we have an user name.
                if (string.IsNullOrWhiteSpace(user.Name))
                    throw new Exception("Name is required");

                // Insert/update users.
                var result = await conn.InsertOrReplaceAsync(user).ConfigureAwait(continueOnCapturedContext: false);
                StatusMessage = $"{result} record(s) added [User Name: {user.Name}])";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to create user: {user.Name}. Error: {ex.Message}";
            }
        }

        public Task<List<User>> GetAllUsers()
        {
            // Return a list of users saved to the users table in the database.
            return conn.Table<User>().ToListAsync();
        }
    }
}
