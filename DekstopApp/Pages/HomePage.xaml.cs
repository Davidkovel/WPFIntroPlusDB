using System.Windows;
using System.Windows.Controls;
using Core.Repository.BorrowRecordsRepository;
using DatabaseService.DBProvider;
using DatabaseService.Models;
using DatabaseService.Repository;

namespace DesktopApp.Pages;

public partial class HomePage : UserControl
{
    UserModel _user;
    private DatabaseProvider? _databaseProvider;
    private BorrowRecord? _peopleBorrowRecords;
    private BorrowRecordsRepository<IEnumerable<dynamic>?> _borrowRecordsRepository;

    public HomePage(UserModel user, DatabaseProvider? databaseProvider)
    {
        _user = user;
        _databaseProvider = databaseProvider;
        _borrowRecordsRepository = new BorrowRecordsRepositoryImpl(databaseProvider);
        InitializeComponent();
    }

    private async void OnAddClick(object sender, RoutedEventArgs e)
    {
        try
        {
            string bookTitle = tBookTitle.Text;
            string bookAuthor = TBookAuthor.Text;
        
            if (string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrWhiteSpace(bookAuthor))
            {
                MessageBox.Show("Please enter both book title and author", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool success = await _borrowRecordsRepository.AddBorrowRecordsByUserLogin(
                _user.Login, bookTitle, bookAuthor);
        
            if (success)
            {
                MessageBox.Show("Book borrowed successfully!", "Success", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                await RefreshDataGrid();
            }
            else
            {
                MessageBox.Show("Failed to borrow book. Please check if the book exists.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "System Exception", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task RefreshDataGrid()
    {
        var items = await _borrowRecordsRepository.GetBorrowRecordsByUserLogin(_user.Login);
        dgMarkAndSubject.ItemsSource = items;
        dgMarkAndSubject.UpdateLayout();
    }

    private async void HomePage_OnLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
            await RefreshDataGrid();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "System Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}