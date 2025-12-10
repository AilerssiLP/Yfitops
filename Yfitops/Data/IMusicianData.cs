using Yfitops.Models;

namespace Yfitops.Data;
public interface IMusicianData
{
    Task AddAsync(Musician musician);
    Task<Musician?> GetByUserIdAsync(int userId);
}
