namespace DatabaseService.DBCommands;

public class DbCommands
{
    public static string CreateDbCommandWithNotExists(string dbName) =>
        $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{dbName}') CREATE DATABASE {dbName};";

    public static string UseDbCommand(string dbName) => $"USE {dbName};";

    public static string CreateTablesCommandIfNotExist() => @"
            IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
            BEGIN
                CREATE TABLE Users (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Login NVARCHAR(100) NOT NULL UNIQUE,
                    Password NVARCHAR(100) NOT NULL
                );
            END

            IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Books')
            BEGIN
                CREATE TABLE Books (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Title NVARCHAR(255) NOT NULL,
                    Author NVARCHAR(255) NULL,
                    PublicationYear INT NULL,
                    Genre NVARCHAR(100) NULL,
                    IsAvailable BIT NULL
                );
            END

            IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BorrowRecords')
            BEGIN
                CREATE TABLE BorrowRecords (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    BookId INT NOT NULL,
                    UserId INT NOT NULL,
                    BorrowDate DATETIME NOT NULL,
                    ReturnDate DATETIME NULL,
                    IsReturned BIT NULL,
                    CONSTRAINT FK_BorrowRecords_Books FOREIGN KEY (BookId) REFERENCES Books(Id) ON DELETE CASCADE,
                    CONSTRAINT FK_BorrowRecords_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
                );
            END

            IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'People')
            BEGIN
                CREATE TABLE People (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Name NVARCHAR(100) NULL,
                    Surname NVARCHAR(100) NULL
                );
            END

            IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Subjects')
            BEGIN
                CREATE TABLE Subjects (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Name NVARCHAR(100) NOT NULL UNIQUE
                );
            END

            IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Grades')
            BEGIN
                CREATE TABLE Grades (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    UserId INT NOT NULL,
                    SubjectId INT NOT NULL,
                    Mark INT NOT NULL,
                    CONSTRAINT FK_Grades_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
                    CONSTRAINT FK_Grades_Subjects FOREIGN KEY (SubjectId) REFERENCES Subjects(Id) ON DELETE CASCADE
                );
            END
        ";

    public static string DropTablesCommand() => @"
            DROP TABLE IF EXISTS BorrowRecords;
            DROP TABLE IF EXISTS Books;
            DROP TABLE IF EXISTS Users;
            DROP TABLE IF EXISTS People;
            ";

    public static string InsertUserCommand(string login, string password)
    {
        return $"USE Library; INSERT INTO Users (Login, Password) VALUES ({login}, {password});";
    }

    public static string GetUserByLoginCommand()
    {
        return "USE Library; SELECT * FROM Users WHERE login = @login;";
    }

    public static string InsertBorrowRecordCommand(string login, string bookTitle, string bookAuthor)
    {
        return $@"
        USE Library;
        
        DECLARE @userId INT;
        SELECT @userId = Id FROM Users WHERE Login = '{login}';
        
        DECLARE @bookId INT;
        SELECT @bookId = Id FROM Books WHERE Title = '{bookTitle}' AND Author = '{bookAuthor}' AND IsAvailable = 1;
        
        IF @userId IS NOT NULL AND @bookId IS NOT NULL
        BEGIN
            BEGIN TRANSACTION;
            
            INSERT INTO BorrowRecords (BookId, UserId, BorrowDate) 
            VALUES (@bookId, @userId, GETDATE());
            
            UPDATE Books SET IsAvailable = 0 WHERE Id = @bookId;
            
            COMMIT TRANSACTION;
            SELECT 1; -- Успех
        END
        ELSE
        BEGIN
            SELECT 0; -- Неудача (пользователь не найден или книга недоступна)
        END";
    }

    public static string ReturnBookCommand(int recordId)
    {
        return $@"
            USE Library;
            UPDATE BorrowRecords 
            SET ReturnDate = GETDATE(), IsReturned = 1 
            WHERE Id = {recordId};
            
            UPDATE Books b
            SET b.IsAvailable = 1
            FROM Books b
            JOIN BorrowRecords br ON b.Id = br.BookId
            WHERE br.Id = {recordId};";
    }

    public static string GetBorrowRecordsByUserLogin(string login)
    {
        return $@"
            USE Library;
            SELECT 
                br.Id AS RecordId,
                b.Id AS BookId,
                b.Title AS BookTitle,
                b.Author AS BookAuthor,
                u.Id AS UserId,
                br.BorrowDate,
                br.ReturnDate,
                br.IsReturned
            FROM 
                BorrowRecords br
            JOIN 
                Books b ON br.BookId = b.Id
            JOIN 
                Users u ON br.UserId = u.Id
            WHERE 
                u.Login = '{login}'
            ORDER BY 
                br.BorrowDate DESC;";
    }

    public static string GetAvailableBooksCommand()
    {
        return @"
            USE Library;
            SELECT * FROM Books 
            WHERE IsAvailable = 1
            ORDER BY Title;";
    }
}