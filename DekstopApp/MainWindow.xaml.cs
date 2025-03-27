using System.Data.SqlClient;
using System.Windows;
using DatabaseService.DBProvider;

namespace DesktopApp;

public partial class MainWindow : Window
{
    private DatabaseProvider _provider;
    public MainWindow()
    {
        InitializeComponent();
        const string _password = "123456I!@";
        string connectionString =
            $"Server=localhost,1434;Database=Library;User Id=sa;Password={_password};TrustServerCertificate=True;";
        _provider = new DatabaseProvider(connectionString);
    }

    [Obsolete("Obsolete")]
    private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
            await _provider.InitializeDatabaseAsync();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            await _provider.ResetDatabaseAsync();
            await _provider.InitializeDatabaseAsync();
        }
        finally
        {
            MainWindowFrame.Navigate(new Pages.LoginPage(_provider));
        }
    }
}