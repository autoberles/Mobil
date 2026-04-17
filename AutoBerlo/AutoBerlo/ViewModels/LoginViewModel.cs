using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoBerlo.Services;
using AutoBerlo.Models;
using AutoBerlo.Pages;

namespace AutoBerlo.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly ApiService _api;

    public LoginViewModel(ApiService api)
    {
        _api = api;
    }

    // ── Login mezők ───────────────────────────────────────────────

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string email = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string password = string.Empty;

    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string errorMessage = string.Empty;

    private bool CanLogin => !string.IsNullOrWhiteSpace(Email)
                          && !string.IsNullOrWhiteSpace(Password)
                          && !IsLoading;

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {
        IsLoading = true;
        ErrorMessage = string.Empty;
        var (success, message) = await _api.LoginAsync(
            new LoginRequest { Email = Email, Password = Password });
        IsLoading = false;

        if (success)
        {
            // Navigálás a főoldalra, vissza nem lehet jönni
            Application.Current!.MainPage = new AppShell();
        }
        else
        {
            ErrorMessage = message;
        }
    }

    [RelayCommand]
    private async Task GoToRegisterAsync()
    {
        // Shell.Current null mert LoginPage NavigationPage-en belül van
        // NavigationPage-en belüli navigációhoz Application.Current.MainPage kell
        if (Application.Current?.MainPage is NavigationPage navPage)
            await navPage.PushAsync(
                IPlatformApplication.Current!.Services.GetRequiredService<Pages.RegisterPage>());
    }

    [RelayCommand]
    private async Task GoToForgotPasswordAsync()
    {
        if (Application.Current?.MainPage is NavigationPage navPage)
            await navPage.PushAsync(
                IPlatformApplication.Current!.Services.GetRequiredService<Pages.ForgotPasswordPage>());
    }
}