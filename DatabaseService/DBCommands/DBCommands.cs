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

    public static string InsertMarkAndSubjectByUserLogin(string login, string subject, int mark)
    {
        return $@"USE Library; 
                                    DECLARE @userId INT;
                                    SELECT @userId = Id FROM Users WHERE Login = {login};
                                    DECLARE @studentId INT;
                                    SELECT @studentId = Id FROM Students WHERE UserId = @userId;
                                    DECLARE @markId INT;
                                    SELECT @markId = Id FROM Marks WHERE StudentId = @studentId;
                                    DECLARE @subjectId INT;
                                    SELECT @subjectId = Id FROM Subjects WHERE Name = {subject};
                                    INSERT INTO MarksDetail (MarkId, Mark) VALUES (@markId, {mark});
                                    INSERT INTO StudentSubjects (StudentId, SubjectId) VALUES (@studentId, @subjectId);";
    }

    public static string GetSubjectAndMarksByUserLogin(string login)
    {
        return $@"
        USE Library;
        DECLARE @userId INT;
        SELECT @userId = Id FROM Users WHERE Login = '{login}';
        DECLARE @studentId INT;
        SELECT @studentId = Id FROM Students WHERE UserId = @userId;
        WITH SubjectMarks AS (
            SELECT 
                s.Id AS SubjectId, 
                s.Name AS SubjectName, 
                md.Mark
            FROM 
                Subjects s
            INNER JOIN 
                StudentSubjects ss ON s.Id = ss.SubjectId
            INNER JOIN 
                MarksDetail md ON md.MarkId = ss.SubjectId
            WHERE 
                ss.StudentId = @studentId
        )
        SELECT 
            SubjectId, 
            SubjectName, 
            Mark
        FROM 
            SubjectMarks;";
    }
}