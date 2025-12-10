using Yfitops.Models;

namespace Yfitops.Data;
public interface ISongData
{
    Task AddAsync(Song song);
    Task<IEnumerable<Song>> GetByAlbumAsync(int albumId);
}
