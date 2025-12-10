using Dapper;
using Microsoft.Data.Sqlite;
using Yfitops.Models;

namespace Yfitops.Data;
public class MusicianData : IMusicianData
{
    private readonly string _connection = "Data Source=app.db";

    public async Task AddAsync(Musician musician)
    {
        using var conn = new SqliteConnection(_connection);

        var sql = @"
        INSERT INTO Musicians (UserId, StageName) VALUES (@UserId, @StageName);
        SELECT last_insert_rowid();
        ";

        var id = await conn.ExecuteScalarAsync<long>(sql, musician);
        musician.Id = (int)id;
    }


    public async Task<Musician?> GetByUserIdAsync(int userId)
    {
        using var conn = new SqliteConnection(_connection);
        return await conn.QueryFirstOrDefaultAsync<Musician>(
            "SELECT * FROM Musicians WHERE UserId = @UserId",
            new { UserId = userId }
        );
    }
}
