using Core.Repository.AuthRepository;
using DatabaseService.DBProvider;
using DatabaseService.Models;

namespace DatabaseService.Repository;

public class AuthRepositoryImpl : AuthRepository
{
    private readonly DatabaseProvider _databaseProvider;
    
    public AuthRepositoryImpl(DatabaseProvider databaseProvider)
    {
        _databaseProvider = databaseProvider;
    }
    public override Task<bool> RegisterAsync(string email, string hashed_password)
    {
        UserModel user = new UserModel(email, hashed_password);
        var resultResponse = _databaseProvider.CreateUserAsync(user);
        return Task.FromResult(resultResponse.Result == 1);
    }       
    
    public override Task<bool> LoginAsync(string email, string hashed_password)
    {
        var user = _databaseProvider.GetUserAsync(email);
        return Task.FromResult(user.Result?.PasswordHash == hashed_password);
    }
}