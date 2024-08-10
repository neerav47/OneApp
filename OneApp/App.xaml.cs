using OneApp.Views;

namespace OneApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = (Page)CheckStatus();
        }

        object CheckStatus()
        {
            //var userInfo = SecureStorage.Default.GetAsync("UserInfo").GetAwaiter().GetResult();
            var userInfo = Preferences.Default.Get<string>("UserInfo", null);
            if (userInfo == null)
            {
                return new LoginPage();
            }
            else
            {
                return new AppShell();
            }
        }
    }
}