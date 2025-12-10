using Yfitops.Models;

namespace Yfitops.Data;
public interface IAlbumData
{
    Task AddAsync(Album album);
    Task<IEnumerable<Album>> GetByMusicianAsync(int musicianId);
}
