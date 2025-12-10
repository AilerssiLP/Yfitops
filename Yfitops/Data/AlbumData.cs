using Dapper;
using Microsoft.Data.Sqlite;
using Yfitops.Models;

namespace Yfitops.Data;
public class AlbumData : IAlbumData
{
    private readonly string _connection = "Data Source=app.db";

    public async Task AddAsync(Album album)
    {
        using var conn = new SqliteConnection(_connection);
        await conn.ExecuteAsync(
            "INSERT INTO Albums (MusicianId, Title) VALUES (@MusicianId, @Title)",
            album
        );
    }

    public async Task<IEnumerable<Album>> GetByMusicianAsync(int musicianId)
    {
        using var conn = new SqliteConnection(_connection);
        return await conn.QueryAsync<Album>(
            "SELECT * FROM Albums WHERE MusicianId = @MusicianId",
            new { MusicianId = musicianId }
        );
    }
}
