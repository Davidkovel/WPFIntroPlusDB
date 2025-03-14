namespace Core.Entity;

public class BorrowRecord
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public bool IsReturned { get; set; } = false;
}