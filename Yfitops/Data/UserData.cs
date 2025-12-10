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

        return await conn.QueryAsync<User>(
            "SELECT Id, Username, PasswordHash, Salt, Role FROM Users;"
        );
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
        return await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Username = @Username;", 
        new { Username = username });
    }
    public async Task UpdateRoleAsync(User user)
    {
        using var conn = new SqliteConnection(_connectionString);

        await conn.ExecuteAsync("UPDATE Users SET Role = @Role WHERE Id = @Id",
        new { user.Role, user.Id });
    }

    public async Task DeleteAsync(int userId)
    {
        using var conn = new SqliteConnection(_connectionString);

        await conn.ExecuteAsync(@"
        DELETE FROM Songs
        WHERE AlbumId IN (
            SELECT Id FROM Albums
            WHERE MusicianId IN (
                SELECT Id FROM Musicians WHERE UserId = @UserId
            )
        )", new { UserId = userId });


        await conn.ExecuteAsync(@"
        DELETE FROM Albums
        WHERE MusicianId IN (
            SELECT Id FROM Musicians WHERE UserId = @UserId
        )", new { UserId = userId });


        await conn.ExecuteAsync(@"
        DELETE FROM Musicians
        WHERE UserId = @UserId",
            new { UserId = userId });


        await conn.ExecuteAsync(
            "DELETE FROM Users WHERE Id = @UserId",
            new { UserId = userId });
    }



}
