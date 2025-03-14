namespace DatabaseService.Models;

public class BorrowRecordModel
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool IsReturned { get; set; }

    public BorrowRecordModel(int id, int bookId, int userId, DateTime borrowDate, DateTime? returnDate, bool isReturned)
    {
        Id = id;
        BookId = bookId;
        UserId = userId;
        BorrowDate = borrowDate;
        ReturnDate = returnDate;
        IsReturned = isReturned;
    }

    public override string ToString()
    {
        return $"Id: {Id}, BookId: {BookId}, UserId: {UserId}, BorrowDate: {BorrowDate}, ReturnDate: {ReturnDate}, IsReturned: {IsReturned}";
    }
}