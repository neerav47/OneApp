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
            //var result = Task.Run(authenticationService.GetUserContext).Result;
            MainPage = new ContentPage();
        }

        protected override async void OnStart()
        {
            try
            {
                Console.WriteLine($"OnStart executing: {nameof(OnStart)}");
                var userContext = await this._authenticationService.RefreshUserContext();
                if (userContext is null)
                {
                    MainPage = _loginPage;
                }
                else
                {
                    MainPage = _appShell;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occured: {ex.Message}");
                MainPage = _loginPage;
            }
        }

        // protected override Window CreateWindow(IActivationState activationState)
        // {
        //     Window window = base.CreateWindow(activationState);
        //
        //     // Created
        //     window.Created += (s, e) =>
        //     {
        //         Console.WriteLine("Window Created");
        //         //MainPage = Task.Run(_authenticationService.GetUserContext).Result != null ? _appShell : _loginPage;
        //         MainPage = Task.Run(_authenticationService.RefreshUserContext).Result != null ? _appShell : _loginPage;
        //     };
        //
        //     //Stopped
        //     window.Stopped += (s, e) =>
        //     {
        //         Console.WriteLine("Window Stopped");
        //     };
        //
        //     // Resumed
        //     window.Resumed += (s, e) =>
        //     {
        //         Console.WriteLine("Window Resumed");
        //         _authenticationService.RefreshUserContext();
        //     };
        //     return window;
        // }
    }
}