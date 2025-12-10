namespace Yfitops.Models;

public enum UserRole
{
    Admin,
    Musician,
    User
}

public class User
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
    public string? Salt { get; set; }
    public UserRole Role { get; set; }
}
