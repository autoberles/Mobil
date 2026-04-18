using AutoBerlo.Pages;
using AutoBerlo.Services;

namespace AutoBerlo;

public partial class App : Application
{
    public App(IServiceProvider services, AuthService authService)
    {
        InitializeComponent();

        if (authService.IsLoggedIn)
        {
            MainPage = new AppShell();
        }
        else
        {
            var loginPage = services.GetRequiredService<LoginPage>();
            var navPage = new NavigationPage(loginPage)
            {
                BarBackgroundColor = Color.FromArgb("#1A1A1A"),
                BarTextColor = Color.FromArgb("#FFFF29")
            };
            MainPage = navPage;
        }
    }
}