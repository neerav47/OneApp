namespace OneApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

	void OnForgotPasswordTap(object sender, EventArgs eventArgs)
	{
		DisplayAlert("Forgot password", "Are you sure to change password", "Ok", "Cancel");
	}
}
