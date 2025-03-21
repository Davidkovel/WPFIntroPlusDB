using System.Data.SqlClient;
using Dapper;
using DatabaseService.DBCommands;
using DatabaseService.Models;

namespace DatabaseService.DBProvider;

public class DatabaseProvider
{
    private readonly string _connectionString;
        
    public DatabaseProvider(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task<int> InsertMarkAndSubjectByUserLogin(string login, string subject, int mark)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("USE Library");
        return await connection.ExecuteAsync(DbCommands.InsertMarkAndSubjectByUserLogin(login, subject, mark));
    }

    public async Task<BorrowRecord?> GetSubjectAndMarksByUserLogin(string login)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("USE Library");
        //!TODO Fix: Add a query to get all subjects and marks by user login
        var result = await connection.QueryAsync(DbCommands.GetSubjectAndMarksByUserLogin(login));
        var enumerable = result.ToList();
        if(enumerable.ToList().Count > 0)
        {
            return null;
        }
        BorrowRecord borrowRecord = new BorrowRecord();
        foreach (var row in enumerable)
        {
            borrowRecord.marks.Add(row.Subject, row.Mark);
        }
        return borrowRecord;
    }
        
    [Obsolete("Obsolete")]
    public async Task<int> CreateUserAsync(UserModel user)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<int>(DbCommands.InsertUserCommand(user.Login, user.PasswordHash));
    }
        
    [Obsolete("Obsolete")]
    public async Task<UserModel?> GetUserAsync(string userLogin)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("USE Library");
        var user = await connection.QuerySingleOrDefaultAsync<UserModel>("SELECT * FROM dbo.Users WHERE Login = @login", new { login = userLogin } );
        
        return user;
    }
    
    [Obsolete("Obsolete")]
    public async Task InitializeDatabaseAsync()
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await connection.ExecuteAsync(DbCommands.CreateDbCommandWithNotExists("Library"));
            await connection.ExecuteAsync(DbCommands.CreateTablesCommandIfNotExist());
            // await connection.ExecuteAsync(@"
            //     IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Library' AND xtype='U')
            //     CREATE TABLE Users (
            //         id INT PRIMARY KEY IDENTITY,
            //         login NVARCHAR(50) NOT NULL,
            //         password NVARCHAR(50) NOT NULL
            //     )");
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            throw new Exception("Database initialization failed");
        }
    }
        
    [Obsolete("Obsolete")]
    public async Task ResetDatabaseAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(DbCommands.DropTablesCommand());
        await connection.ExecuteAsync(DbCommands.CreateTablesCommandIfNotExist());
    }
}