using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OneApp.Views;
using OneApp.Views.User;

namespace OneApp.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly ILogger<SettingsViewModel> _logger;
    private readonly LoginPage _loginPage;

    public SettingsViewModel(ILogger<SettingsViewModel> logger, IServiceProvider serviceProvider)
    {
        LoadSettingItems();
        this._logger = logger;
        this._loginPage = serviceProvider.GetService<LoginPage>();
    }
    
    [ObservableProperty]
    IEnumerable<SettingItem> _items;
    
    [RelayCommand]
    private async Task SettingItemSelected(SettingItem settingItem)
    {
        _logger.LogInformation($"{nameof(SettingItemSelected)} started.");
        if (settingItem.PageName.Equals("Logout"))
        {
            this.SignOut();
        }
        else
        {
            await Shell.Current.GoToAsync($"{settingItem.Name}", true);
        }
    }

    private void SignOut()
    {
        SecureStorage.Default.RemoveAll();
        Application.Current!.MainPage = _loginPage;
    }

    private void LoadSettingItems()
    {
        Items = new List<SettingItem>
        {
            new SettingItem()
            {
                Name = "Users",
                Description = "Create, edit users",
                IconName = "users.png",
                PageName = "Users"
            },
            new SettingItem()
            {
                Name = "Categories",
                Description = "Create, edit category",
                IconName = "collection.png",
                PageName = "Categories"
            },
            new SettingItem()
            {
                Name = "Units",
                Description = "Create, edit units",
                IconName = "unit.png",
                PageName = "Units"
            },
            new SettingItem()
            {
                Name = "Log out",
                IconName = "logout.png",
                PageName = "Logout"
            }
        };
    }
}

public class SettingItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconName { get; set; }
    public string PageName { get; set; }
}