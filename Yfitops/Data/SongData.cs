using Dapper;
using Microsoft.Data.Sqlite;
using Yfitops.Models;

namespace Yfitops.Data;
public class SongData : ISongData
{
    private readonly string _connection = "Data Source=app.db";

    public async Task AddAsync(Song song)
    {
        using var conn = new SqliteConnection(_connection);
        await conn.ExecuteAsync(
            "INSERT INTO Songs (AlbumId, Title, FilePath) VALUES (@AlbumId, @Title, @FilePath)",
            song
        );
    }

    public async Task<IEnumerable<Song>> GetByAlbumAsync(int albumId)
    {
        using var conn = new SqliteConnection(_connection);
        return await conn.QueryAsync<Song>(
            "SELECT * FROM Songs WHERE AlbumId = @AlbumId",
            new { AlbumId = albumId }
        );
    }
    public async Task DeleteByAlbumAsync(int albumId)
    {
        using var conn = new SqliteConnection(_connection);
        await conn.ExecuteAsync("DELETE FROM Songs WHERE AlbumId = @AlbumId", new { AlbumId = albumId });
    }

    public async Task DeleteAsync(int songId)
    {
        using var conn = new SqliteConnection(_connection);
        await conn.ExecuteAsync("DELETE FROM Songs WHERE Id = @Id", new { Id = songId });
    }


}
