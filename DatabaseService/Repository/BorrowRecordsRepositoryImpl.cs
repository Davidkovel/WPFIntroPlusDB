using Core.Repository.BorrowRecordsRepository;
using DatabaseService.DBProvider;
using DatabaseService.Models;

namespace DatabaseService.Repository;

public class BorrowRecordsRepositoryImpl : BorrowRecordsRepository<IEnumerable<dynamic>?>
{
    private readonly DatabaseProvider _databaseProvider;

    public BorrowRecordsRepositoryImpl(DatabaseProvider databaseProvider)
    {
        _databaseProvider = databaseProvider;
    }

    public override async Task<IEnumerable<dynamic>?> GetBorrowRecordsByUserLogin(string login)
    {
        var result = await _databaseProvider.GetSubjectAndMarksByUserLogin(login);
        return result;
    }

    public override async Task<bool> AddBorrowRecordsByUserLogin(string login, string bookTitle, string bookAuthor)
    {
        try
        {
            int result = await _databaseProvider.InsertBorrowRecordsByUserLogin(login, bookTitle, bookAuthor);
            return result > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while adding borrow record: {ex.Message}");
            return false;
        }
    }
}