using System.Security.Cryptography;
using System.Text;
using Yfitops.Models;
using Yfitops.Data;

namespace Yfitops.Services;

public class UserService
{
    private readonly IUserData _repo;

    public User? CurrentUser { get; private set; }

    public UserService(IUserData repo)
    {
        _repo = repo;
    }

    private static string GenerateSalt()
    {
        var bytes = new byte[16];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }

    private static string HashPassword(string password, string salt)
    {
        var combined = Encoding.UTF8.GetBytes(password + salt);
        var hash = SHA256.HashData(combined);
        return Convert.ToBase64String(hash);
    }

    public async Task<bool> Register(string username, string password, UserRole role)
    {
        if (await _repo.GetByUsernameAsync(username) != null)
            return false;

        var salt = GenerateSalt();
        var hash = HashPassword(password, salt);

        var user = new User
        {
            Username = username,
            PasswordHash = hash,
            Salt = salt,
            Role = role
        };

        await _repo.AddAsync(user);
        return true;
    }
    public static string HashPasswordStatic(string password, string salt)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password + salt);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public async Task<bool> Login(string username, string password)
    {
        var user = await _repo.GetByUsernameAsync(username);
        if (user == null) return false;

        var hash = HashPassword(password, user.Salt);

        if (hash != user.PasswordHash)
            return false;

        CurrentUser = user;
        return true;
    }

    public void Logout()
    {
        CurrentUser = null;
    }
}
