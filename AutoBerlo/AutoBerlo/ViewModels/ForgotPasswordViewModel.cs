using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoBerlo.Services;
using AutoBerlo.Models;

namespace AutoBerlo.ViewModels;

public partial class ForgotPasswordViewModel : ObservableObject
{
    private readonly ApiService _api;

    public ForgotPasswordViewModel(ApiService api)
    {
        _api = api;
    }

    [ObservableProperty] private string email = string.Empty;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string errorMessage = string.Empty;

    [ObservableProperty] private bool codeStep;
    [ObservableProperty] private string code = string.Empty;
    [ObservableProperty] private string newPassword = string.Empty;
    [ObservableProperty] private string confirmNewPassword = string.Empty;
    [ObservableProperty] private string successMessage = string.Empty;

    [RelayCommand]
    private async Task SendCodeAsync()
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            ErrorMessage = "Add meg az email címet!";
            return;
        }
        IsLoading = true;
        ErrorMessage = string.Empty;
        var (success, message) = await _api.ForgotPasswordAsync(Email);
        IsLoading = false;

        if (success)
            CodeStep = true;
        else
            ErrorMessage = message;
    }

    [RelayCommand]
    private async Task ResetPasswordAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Code) || string.IsNullOrWhiteSpace(NewPassword))
        {
            ErrorMessage = "A kód és az új jelszó megadása kötelező!";
            return;
        }
        if (NewPassword != ConfirmNewPassword)
        {
            ErrorMessage = "A két jelszó nem egyezik!";
            return;
        }

        IsLoading = true;
        var (success, message) = await _api.ResetPasswordAsync(new ResetPasswordRequest
        {
            Email = Email,
            Code = Code,
            NewPassword = NewPassword
        });
        IsLoading = false;

        if (success)
        {
            SuccessMessage = message;
            await Task.Delay(1500);
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            ErrorMessage = message;
        }
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        if (Application.Current?.MainPage is NavigationPage navPage)
            await navPage.PopAsync();
    }
}