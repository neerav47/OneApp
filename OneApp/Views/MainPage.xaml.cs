namespace OneApp.Views;

public partial class MainPage : ContentPage
{
    private readonly LoginPage _loginPage;
    public MainPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        this._loginPage = serviceProvider.GetService<LoginPage>();
    }

    void OnSalesTap(object sender, EventArgs eventArgs)
    {
        Navigate(new SalesPage());
    }

    void OnInventoryTap(object sender, EventArgs eventArgs)
    {
        Navigate(new InventoryPage());
    }

    void OnReportTap(object sender, EventArgs eventArgs)
    {
        Navigate(new ReportsPage());
    }

    void OnSignOutTap(object sender, EventArgs eventArgs)
    {
        SecureStorage.Default.RemoveAll();
        Application.Current.MainPage = _loginPage;
    }

    private async void Navigate(ContentPage page)
    {
        await Navigation.PushAsync(page);
    }
}