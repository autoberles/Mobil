using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AutoBerlo.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    string email;

    [ObservableProperty]
    string password;

    [RelayCommand]
    async Task Login()
    {
        await Application.Current.MainPage.DisplayAlert(
            "Login",
            $"Email: {Email}",
            "OK");
    }

    [RelayCommand]
    async Task GoToRegister()
    {
        await Shell.Current.GoToAsync("register");
    }
}