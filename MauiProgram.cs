using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace TradingJournalApp;

/// <summary>
/// Main entry point for the application.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Creates the Maui application.
    /// </summary>
    /// <returns></returns>
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        LiveCharts.Configure(config =>
        {
            config
                .AddSkiaSharp()
                .AddDefaultMappers();
        });

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
