using OneApp.ViewModels;

namespace OneApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel _loginViewModel)
	{
        InitializeComponent();
        BindingContext = _loginViewModel;
	}
}
