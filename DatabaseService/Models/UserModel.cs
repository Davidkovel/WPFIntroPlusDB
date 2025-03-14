using Core.Entity;

namespace DatabaseService.Models;

public class UserModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }

    public UserModel(string email, string hashed_password)
    {
        Email = email;
        PasswordHash = hashed_password;
    }
    
    public UserModel(string username, string passwordHash, string email, UserRole role)
    {
        Username = username;
        PasswordHash = passwordHash;
        Email = email;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Username: {Username}, Email: {Email}, Role: {Role}, CreatedAt: {CreatedAt}";
    }
}