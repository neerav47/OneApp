﻿namespace OneApp.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
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
        //SecureStorage.Default.RemoveAll();
        Preferences.Remove("UserInfo");
        App.Current.MainPage = new LoginPage();
    }

    private async void Navigate(ContentPage page)
    {
        await Navigation.PushAsync(page);
    }
}