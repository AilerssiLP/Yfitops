using Yfitops.Models;

namespace Yfitops.Data;
public interface ISongData
{
    Task AddAsync(Song song);
    Task DeleteAsync(int songId);
    Task DeleteByAlbumAsync(int albumId);

    Task<IEnumerable<Song>> GetByAlbumAsync(int albumId);
}
