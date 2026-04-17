using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoBerlo.Services;
using AutoBerlo.Models;

namespace AutoBerlo.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly ApiService _api;

    public RegisterViewModel(ApiService api)
    {
        _api = api;
    }

    [ObservableProperty] private string firstName = string.Empty;
    [ObservableProperty] private string lastName = string.Empty;
    [ObservableProperty] private string email = string.Empty;
    [ObservableProperty] private string password = string.Empty;
    [ObservableProperty] private string confirmPassword = string.Empty;
    [ObservableProperty] private string phoneNumber = string.Empty;
    [ObservableProperty] private DateTime birthDate = DateTime.Today.AddYears(-18);
    [ObservableProperty] private bool acceptTerms;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string errorMessage = string.Empty;
    [ObservableProperty] private string successMessage = string.Empty;

    [RelayCommand]
    private async Task RegisterAsync()
    {
        ErrorMessage = string.Empty;

        // Kliens oldali validáció
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName)
            || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password)
            || string.IsNullOrWhiteSpace(PhoneNumber))
        {
            ErrorMessage = "Minden mező kitöltése kötelező!";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "A két jelszó nem egyezik!";
            return;
        }

        if (Password.Length < 6)
        {
            ErrorMessage = "A jelszó legalább 6 karakter legyen!";
            return;
        }

        // Jelszó komplexitás ellenőrzés — backend ezt is validálja
        if (!Password.Any(char.IsUpper) || !Password.Any(char.IsLower) || !Password.Any(char.IsDigit))
        {
            ErrorMessage = "A jelszónak tartalmaznia kell kisbetűt, nagybetűt és számot!";
            return;
        }

        if (!AcceptTerms)
        {
            ErrorMessage = "El kell fogadnod a felhasználási feltételeket!";
            return;
        }

        IsLoading = true;
        var (success, message) = await _api.RegisterAsync(new RegisterRequest
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Password = Password,
            ConfirmPassword = ConfirmPassword,
            PhoneNumber = FormatPhoneNumber(PhoneNumber),
            BirthDate = BirthDate.ToString("yyyy-MM-dd"),
            AcceptTerms = true
        });
        IsLoading = false;

        if (success)
        {
            SuccessMessage = message;
            await Task.Delay(1500);
            if (Application.Current?.MainPage is NavigationPage navPage)
                await navPage.PopAsync();
        }
        else
        {
            ErrorMessage = message;
        }
    }

    // Regisztráció előtt formázzuk a telefonszámot ha szükséges
    // +36305397785 → +36 30 539 7785
    private string FormatPhoneNumber(string phone)
    {
        // Eltávolítjuk a szóközöket és ellenőrizzük
        var clean = phone.Replace(" ", "").Trim();

        // Ha már jó formátumban van, visszaadjuk
        if (System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\+36\s\d{2}\s\d{3}\s\d{4}$"))
            return phone;

        // +36XXXXXXXXX → +36 XX XXX XXXX
        if (clean.StartsWith("+36") && clean.Length == 12)
            return $"+36 {clean[3..5]} {clean[5..8]} {clean[8..12]}";

        return phone; // Ha nem tudjuk formázni, visszaadjuk eredeti formában
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        if (Application.Current?.MainPage is NavigationPage navPage)
            await navPage.PopAsync();
    }
}