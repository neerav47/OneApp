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

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        UserSignIn();
    }

	private async void UserSignIn()
	{
		await SecureStorage.Default.SetAsync("UserInfo", "test");
		App.Current.MainPage = new AppShell();
	}
}
