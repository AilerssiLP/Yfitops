using Yfitops.Models;

namespace Yfitops.Data;

public interface IUserData
{
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task<User?> GetByUsernameAsync(string username);
    Task UpdateRoleAsync(User user);
    Task DeleteAsync(int userId);

}
