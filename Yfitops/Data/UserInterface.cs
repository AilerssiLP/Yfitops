using Dapper;
using Microsoft.Data.Sqlite;
using Yfitops.Models;

namespace Yfitops.Data;

public class UserData: IUserData
{
    private readonly string _connectionString = "Data Source=app.db";

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var conn = new SqliteConnection(_connectionString);
        return await conn.QueryAsync<User>("SELECT Id, Username FROM Users;");
    }

    public async Task AddAsync(User user)
    {
        using var conn = new SqliteConnection(_connectionString);
        await conn.ExecuteAsync("INSERT INTO Users (Username, PasswordHash, Salt, Role) " +
        "VALUES (@Username, @PasswordHash, @Salt, @Role)", user);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var conn = new SqliteConnection(_connectionString);
        return await conn.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Username = @Username;", 
            new { Username = username }
        );
    }

}
