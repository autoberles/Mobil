using CommunityToolkit.Maui;
using AutoBerlo.Services;
using AutoBerlo.ViewModels;
using AutoBerlo.Pages;

namespace AutoBerlo;

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
                fonts.AddFont("Rajdhani-Regular.ttf", "RajdhaniRegular");
                fonts.AddFont("Rajdhani-Bold.ttf", "RajdhaniBold");
                fonts.AddFont("Rajdhani-SemiBold.ttf", "RajdhaniSemiBold");
            });

        builder.Services.AddSingleton<HttpClient>(_ =>
        {
            var client = new HttpClient();

            if (DeviceInfo.Platform == DevicePlatform.Android)
                client.BaseAddress = new Uri("http://localhost:5128/");
            else
                client.BaseAddress = new Uri("http://localhost:5128/");

            return client;
        });

        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<LookupService>();

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<ForgotPasswordViewModel>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<CarDetailViewModel>();
        builder.Services.AddTransient<PaymentViewModel>();

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<ForgotPasswordPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<CarDetailPage>();
        builder.Services.AddTransient<PaymentPage>();

        return builder.Build();
    }
}