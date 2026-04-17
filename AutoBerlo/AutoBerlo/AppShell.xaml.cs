using AutoBerlo.Pages;

namespace AutoBerlo;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Navigációs útvonalak regisztrálása
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
        Routing.RegisterRoute(nameof(CarDetailPage), typeof(CarDetailPage));
        Routing.RegisterRoute(nameof(PaymentPage), typeof(PaymentPage));
    }
}