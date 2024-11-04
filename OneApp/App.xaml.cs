using OneApp.Extentions;
using OneApp.Services.Interfaces;
using OneApp.Views;

namespace OneApp
{
    public partial class App : Application
    {
        private readonly LoginPage _loginPage;
        private readonly AppShell _appShell;
        public App(IServiceProvider serviceProvider, IAuthenticationService authenticationService)
        {
            InitializeComponent();
            this._loginPage = serviceProvider.GetService<LoginPage>();
            this._appShell = serviceProvider.GetService<AppShell>();
            MainPage = authenticationService.GetUserContext() != null ? _appShell : _loginPage;
        }
    }
}