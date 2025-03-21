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
    private BorrowRecordsRepository<BorrowRecord?>? _borrowRecordsRepository;

    public HomePage(UserModel user, DatabaseProvider? databaseProvider)
    {
        _user = user;
        _databaseProvider = databaseProvider;
        _borrowRecordsRepository = new BorrowRecordsRepositoryImpl(databaseProvider);
        InitializeComponent();
    }

    private void OnAddClick(object sender, RoutedEventArgs e)
    {
        ;
    }

    void FillDataGrid(BorrowRecord studentSubjectMarks)
    {
        foreach (var subject in studentSubjectMarks.marks)
        {
            var dataGridCells = new List<DataGridCell>();
            var dataGridCell = new DataGridCell();
            dataGridCell.Content = subject.Key;
            dataGridCells.Add(dataGridCell);
            dataGridCell = new DataGridCell();
            dataGridCell.Content = subject.Value;
            dataGridCells.Add(dataGridCell);
            dgMarkAndSubject.Items.Add(dataGridCells);
        }

        dgMarkAndSubject.Items.Refresh();
    }

    private async void HomePage_OnLoaded(object sender, RoutedEventArgs e)
    {
        _peopleBorrowRecords = await _borrowRecordsRepository?.GetBorrowRecordsByUserLogin(_user.Login);
        if (_peopleBorrowRecords != null)
        {
            FillDataGrid(_peopleBorrowRecords);
        }
    }
}