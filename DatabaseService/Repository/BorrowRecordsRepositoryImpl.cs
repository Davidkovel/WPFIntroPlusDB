using Core.Repository.BorrowRecordsRepository;
using DatabaseService.DBProvider;
using DatabaseService.Models;

namespace DatabaseService.Repository;

public class BorrowRecordsRepositoryImpl : BorrowRecordsRepository<BorrowRecord?>
{
    private readonly DatabaseProvider _databaseProvider;

    public BorrowRecordsRepositoryImpl(DatabaseProvider databaseProvider)
    {
        _databaseProvider = databaseProvider;
    }

    public override async Task<BorrowRecord?> GetBorrowRecordsByUserLogin(string login)
    {
        var result = await _databaseProvider.GetSubjectAndMarksByUserLogin(login);
        return result;
    }

    public override async Task<bool> AddBorrowRecordsByUserLogin(string login, string subject, int mark)
    {
        var result = await _databaseProvider.InsertMarkAndSubjectByUserLogin(login, subject, mark);
        return result > 0;
    }
}