using Yfitops.Models;

namespace Yfitops.Data;
public interface IMusicianData
{
    Task AddAsync(Musician musician);
    Task<IEnumerable<Musician>> GetAllAsync();
    Task<Musician?> GetByUserIdAsync(int userId);
}
