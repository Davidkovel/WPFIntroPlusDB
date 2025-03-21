using Core.Entity;
using DatabaseService.Abstractions;

namespace DatabaseService.Models;

public class UserModel : IModel
{
    public int? Id { get; set; }
    public string? Login { get; set; }
    public string? PasswordHash { get; set; }

    public UserModel() {}
    
    public UserModel(int id, string login, string hashed_password)
    {
        this.Id = id;
        this.Login = login;
        this.PasswordHash = hashed_password;
    }
    
}