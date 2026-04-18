using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoBerlo.Models;
using AutoBerlo.Services;
using AutoBerlo.Pages;

namespace AutoBerlo.ViewModels;

[QueryProperty(nameof(CarId), "CarId")]
public partial class CarDetailViewModel : ObservableObject
{
    private readonly ApiService _api;
    private readonly LookupService _lookup;

    public CarDetailViewModel(ApiService api, LookupService lookup)
    {
        _api = api;
        _lookup = lookup;
    }

    [ObservableProperty] private int carId;
    [ObservableProperty] private Car? car;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private DateTime startDate = DateTime.Today;
    [ObservableProperty] private DateTime endDate = DateTime.Today.AddDays(1);
    [ObservableProperty] private int totalDays = 1;
    [ObservableProperty] private int totalPrice;

    [ObservableProperty] private string fuelTypeName = "—";
    [ObservableProperty] private string transmissionName = "—";
    [ObservableProperty] private string categoryName = "—";
    [ObservableProperty] private string wheelDriveName = "—";
    [ObservableProperty] private string branchCity = "—";
    [ObservableProperty] private string branchAddress = "—";
    [ObservableProperty] private string branchPhone = "—";
    [ObservableProperty] private string airConditioningName = "—";

    partial void OnCarIdChanged(int value)
    {
        if (value > 0)
            LoadCarCommand.Execute(null);
    }

    partial void OnStartDateChanged(DateTime value) => RecalculatePrice();
    partial void OnEndDateChanged(DateTime value) => RecalculatePrice();
    partial void OnCarChanged(Car? value)
    {
        RecalculatePrice();
        if (value != null)
            ResolveLookups(value);
    }

    [RelayCommand]
    private async Task LoadCarAsync()
    {
        IsLoading = true;

        await _lookup.LoadAllAsync();

        Car = await _api.GetCarByIdAsync(CarId);
        IsLoading = false;
    }

    private void ResolveLookups(Car car)
    {
        FuelTypeName = _lookup.GetFuelTypeName(car.FuelTypeId);
        TransmissionName = _lookup.GetTransmissionName(car.TransmissionId);
        CategoryName = _lookup.GetCategoryName(car.CarCategoryId);
        WheelDriveName = _lookup.GetWheelDriveName(car.WheelDriveTypeId);
        BranchCity = _lookup.GetBranchCity(car.BranchId);
        BranchAddress = _lookup.GetBranchAddress(car.BranchId);
        BranchPhone = _lookup.GetBranchPhone(car.BranchId);

        if (car.AdditionalEquipment != null)
            AirConditioningName = _lookup.GetAirConditioningName(
                car.AdditionalEquipment.AirConditioningId);
    }

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