namespace Core.Entity;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int PublicationYear { get; set; }
    public string Genre { get; set; }
    public bool IsAvailable { get; set; } = true;

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(Title) &&
               !string.IsNullOrEmpty(Author) &&
               PublicationYear > 0;
    }
}