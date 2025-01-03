﻿using CommunityToolkit.Maui;
using DevExpress.Maui;
using Microsoft.Extensions.Logging;
using OneApp.Extentions;
using OneApp.Services;
using OneApp.Services.Interfaces;
using OneApp.Views;
using The49.Maui.BottomSheet;

namespace OneApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseDevExpress()
            .UseDevExpressCollectionView()
            .UseDevExpressControls()
            .UseDevExpressEditors()
            .UseMauiCommunityToolkit()
            .UseBottomSheet()
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