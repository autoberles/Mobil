using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoBerlo.Services;
using AutoBerlo.Models;

namespace AutoBerlo.ViewModels;

[QueryProperty(nameof(Car), "Car")]
[QueryProperty(nameof(StartDate), "StartDate")]
[QueryProperty(nameof(EndDate), "EndDate")]
[QueryProperty(nameof(TotalPrice), "TotalPrice")]
[QueryProperty(nameof(TotalDays), "TotalDays")]
public partial class PaymentViewModel : ObservableObject
{
    private readonly ApiService _api;

    public PaymentViewModel(ApiService api)
    {
        _api = api;
    }

    [ObservableProperty] private Car? car;
    [ObservableProperty] private DateTime startDate;
    [ObservableProperty] private DateTime endDate;
    [ObservableProperty] private int totalPrice;
    [ObservableProperty] private int totalDays;

    // Szimulált fizetési mezők
    [ObservableProperty] private string cardNumber = string.Empty;
    [ObservableProperty] private string cardHolder = string.Empty;
    [ObservableProperty] private string expiryDate = string.Empty;
    [ObservableProperty] private string cvv = string.Empty;

    [ObservableProperty] private bool isProcessing;
    [ObservableProperty] private bool isSuccess;
    [ObservableProperty] private string errorMessage = string.Empty;

    [RelayCommand]
    private async Task PayAsync()
    {
        ErrorMessage = string.Empty;

        // Kártya validáció (szimuláció)
        if (string.IsNullOrWhiteSpace(CardNumber) || CardNumber.Replace(" ", "").Length < 16)
        {
            ErrorMessage = "Érvénytelen kártyaszám!";
            return;
        }
        if (string.IsNullOrWhiteSpace(CardHolder))
        {
            ErrorMessage = "Add meg a kártyabirtokos nevét!";
            return;
        }
        if (string.IsNullOrWhiteSpace(ExpiryDate))
        {
            ErrorMessage = "Add meg a lejárati dátumot!";
            return;
        }
        if (string.IsNullOrWhiteSpace(Cvv) || Cvv.Length < 3)
        {
            ErrorMessage = "Érvénytelen CVV!";
            return;
        }

        IsProcessing = true;

        // Szimuláljuk a fizetési feldolgozást
        await Task.Delay(2000);

        // Bérlés létrehozása a backendben
        var (success, message, rental) = await _api.CreateRentalAsync(new CreateRentalRequest
        {
            CarId = Car!.Id,
            StartDate = StartDate.ToString("yyyy-MM-dd"),
            EndDate = EndDate.ToString("yyyy-MM-dd")
        });

        IsProcessing = false;

        if (success)
        {
            IsSuccess = true;
            // 2 másodperc után vissza a főoldalra
            await Task.Delay(2500);
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            ErrorMessage = message;
        }
    }

    [RelayCommand]
    private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
}