using gym_rat.Helpers;
using gym_rat.Interfaces;
using gym_rat.Services;
using gym_rat.ViewModels;
using gym_rat.Views;
using Microsoft.Extensions.Logging;

namespace gym_rat
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<DatabaseHelper>(serviceProvider =>
           new DatabaseHelper(Path.Combine(FileSystem.AppDataDirectory, "myapp.db3")));
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddTransient<LoginPage>(sp => new LoginPage(sp.GetRequiredService<LoginViewModel>(), sp));
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<LandingPage>();
            builder.Services.AddTransient<LandingPageViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
