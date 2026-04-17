using AutoBerlo.Pages;
using AutoBerlo.Services;

namespace AutoBerlo;

public partial class App : Application
{
    public App(IServiceProvider services, AuthService authService)
    {
        InitializeComponent();

        if (authService.IsLoggedIn)
            MainPage = new AppShell();
        else
            MainPage = new NavigationPage(services.GetRequiredService<LoginPage>());
    }
}