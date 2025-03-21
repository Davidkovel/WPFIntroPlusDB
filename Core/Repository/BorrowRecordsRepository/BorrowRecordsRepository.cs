namespace Core.Repository.BorrowRecordsRepository;

public abstract class BorrowRecordsRepository<T>
{
    public abstract Task<T> GetBorrowRecordsByUserLogin(string login);
    public abstract Task<bool> AddBorrowRecordsByUserLogin(string login, string subject, int mark);
}