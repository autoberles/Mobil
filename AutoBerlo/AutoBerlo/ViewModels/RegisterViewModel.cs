using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AutoBerlo.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    [ObservableProperty]
    string firstName;

    [ObservableProperty]
    string lastName;

    [ObservableProperty]
    string phone;

    [ObservableProperty]
    string email;

    [ObservableProperty]
    string password;

    [RelayCommand]
    async Task Register()
    {
        await Application.Current.MainPage.DisplayAlert(
            "Regisztráció",
            $"{LastName} {FirstName} sikeresen regisztrált!",
            "OK");

        await Shell.Current.GoToAsync("..");
    }
}