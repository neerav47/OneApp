using OneApp.Extentions;
using OneApp.Services.Interfaces;
using OneApp.Views;

namespace OneApp
{
    public partial class App : Application
    {
        private readonly LoginPage _loginPage;
        private readonly AppShell _appShell;
        private readonly IAuthenticationService _authenticationService;
        public App(IServiceProvider serviceProvider, IAuthenticationService authenticationService)
        {
            InitializeComponent();
            this._loginPage = serviceProvider.GetService<LoginPage>();
            this._appShell = serviceProvider.GetService<AppShell>();
            this._authenticationService = authenticationService;
            var result = Task.Run(authenticationService.GetUserContext).Result;
            MainPage = result != null ? _appShell : _loginPage;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            Window window = base.CreateWindow(activationState);

            // Created
            window.Created += (s, e) =>
            {
                Console.WriteLine("Window Created");
                MainPage = Task.Run(_authenticationService.GetUserContext).Result != null ? _appShell : _loginPage;
            };

            //Stopped
            window.Stopped += (s, e) =>
            {
                Console.WriteLine("Window Stopped");
            };

            // Resumed
            window.Resumed += (s, e) =>
            {
                Console.WriteLine("Window Resumed");
                _authenticationService.RefreshUserContext();
            };
            return window;
        }
    }
}