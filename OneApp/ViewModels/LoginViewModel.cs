using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OneApp.Contracts.v1;
using OneApp.Contracts.v1.Response;
using OneApp.Extentions;
using OneApp.Services.Interfaces;
using System.Collections.ObjectModel;

namespace OneApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthenticationService _authenticationService;
    private readonly AppShell _appShell;
    public LoginViewModel(IAuthenticationService authenticationService, IServiceProvider serviceProvider)
    {
        this._authenticationService = authenticationService;
        this.Users = [];
        this._appShell = serviceProvider.GetService<AppShell>();
    }

    [ObservableProperty]
    string _userName;

    [ObservableProperty]
    string _password;

    [ObservableProperty]
    bool _isLoading = false;

    [ObservableProperty]
    ObservableCollection<User> _users;

    [ObservableProperty]
    User _selectedUser;

    [ObservableProperty]
    bool _isPasswordFieldVisible = false;

    [ObservableProperty]
    bool _isTenantPickerVisible = false;

    [RelayCommand]
    async Task Next()
    {
        if (string.IsNullOrWhiteSpace(UserName))
        {
            return;
        }
        Users.Clear();
        var response = await _authenticationService.GetUser(UserName);

        if (response == null)
        {
            // error?
        }
        else
        {
            foreach(var u in response)
            {
                Users.Add(u);
            }
            SelectedUser = Users[0];
            IsPasswordFieldVisible = true;
            IsTenantPickerVisible = Users.Count > 1;
        }
    }

    [RelayCommand]
    async Task Login()
    {
        IsLoading = true;
        var response = await _authenticationService.Login(new LoginRequest
        {
            UserName = UserName,
            Password = Password,
            TenantId = SelectedUser.Tenant.Id.ToString()
        });

        if (response != null)
        {
            var userContext = new UserContext
            {
                User = SelectedUser,
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken
            };
            await _authenticationService.SetUserContext(userContext);
            Application.Current.MainPage = _appShell;
        }
    }

    [RelayCommand]
    void ForgorPassword()
    {
        Application.Current.MainPage.DisplayAlert("Forgot password", "Are you sure to change password", "Ok", "Cancel");
    }
}