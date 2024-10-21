using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using OneApp.Extentions;
using OneApp.Services;
using OneApp.Services.Interfaces;
using OneApp.Views;

namespace OneApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddHttpClients();
        builder.Services.AddPages();
        builder.Services.AddViewModels();
        builder.Services.AddServies();

        #if DEBUG
		builder.Logging.AddDebug();
        #endif

        return builder.Build();
    }
}