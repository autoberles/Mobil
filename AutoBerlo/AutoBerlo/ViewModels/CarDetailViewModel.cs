using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoBerlo.Models;
using AutoBerlo.Pages;

namespace AutoBerlo.ViewModels;

[QueryProperty(nameof(Car), "Car")]
public partial class CarDetailViewModel : ObservableObject
{
    [ObservableProperty] private Car? car;
    [ObservableProperty] private DateTime startDate = DateTime.Today;
    [ObservableProperty] private DateTime endDate = DateTime.Today.AddDays(1);
    [ObservableProperty] private int totalDays = 1;
    [ObservableProperty] private int totalPrice;

    partial void OnStartDateChanged(DateTime value) => RecalculatePrice();
    partial void OnEndDateChanged(DateTime value) => RecalculatePrice();
    partial void OnCarChanged(Car? value) => RecalculatePrice();

    private void RecalculatePrice()
    {
        if (Car == null) return;
        TotalDays = Math.Max(1, (EndDate.Date - StartDate.Date).Days + 1);
        TotalPrice = TotalDays * Car.DefaultPricePerDay;
    }

    [RelayCommand]
    private async Task ProceedToPaymentAsync()
    {
        if (Car == null) return;

        if (StartDate.Date < DateTime.Today)
        {
            await Shell.Current.DisplayAlert("Hiba", "A bérlés nem kezdődhet a múltban!", "OK");
            return;
        }
        if (EndDate.Date < StartDate.Date)
        {
            await Shell.Current.DisplayAlert("Hiba", "A vége dátum nem lehet korábban a kezdetnél!", "OK");
            return;
        }
        if (!Car.Availability)
        {
            await Shell.Current.DisplayAlert("Hiba", "Ez az autó jelenleg nem elérhető!", "OK");
            return;
        }

        await Shell.Current.GoToAsync(nameof(PaymentPage),
            new Dictionary<string, object>
            {
                ["Car"] = Car,
                ["StartDate"] = StartDate,
                ["EndDate"] = EndDate,
                ["TotalPrice"] = TotalPrice,
                ["TotalDays"] = TotalDays
            });
    }

    [RelayCommand]
    private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
}