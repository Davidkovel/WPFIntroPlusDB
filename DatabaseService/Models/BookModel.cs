namespace DatabaseService.Models;

public class BookModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int PublicationYear { get; set; }
    public string Genre { get; set; }
    public bool IsAvailable { get; set; }

    public BookModel(int id, string title, string author, string isbn, int publicationYear, string genre, bool isAvailable)
    {
        Id = id;
        Title = title;
        Author = author;
        PublicationYear = publicationYear;
        Genre = genre;
        IsAvailable = isAvailable;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Title: {Title}, Author: {Author}, Year: {PublicationYear}, Genre: {Genre}, Available: {IsAvailable}";
    }
}