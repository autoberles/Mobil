using AutoBerlo.Models;
using AutoBerlo.Pages;
using AutoBerlo.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AutoBerlo.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ApiService _api;
    private readonly AuthService _auth;
    private List<Car> _allCars = [];

    public MainViewModel(ApiService api, AuthService auth)
    {
        _api = api;
        _auth = auth;
    }

    // ── Autók listája ─────────────────────────────────────────────
    [ObservableProperty] private ObservableCollection<Car> cars = [];
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private bool isEmpty;
    [ObservableProperty] private string searchText = string.Empty;

    // ── Szűrő opciók ──────────────────────────────────────────────
    [ObservableProperty] private ObservableCollection<string> categories = [];
    [ObservableProperty] private ObservableCollection<string> fuelTypes = [];
    [ObservableProperty] private ObservableCollection<string> transmissionTypes = [];
    [ObservableProperty] private ObservableCollection<string> branches = [];

    [ObservableProperty] private string selectedCategory = "Összes";
    [ObservableProperty] private string selectedFuelType = "Összes";
    [ObservableProperty] private string selectedTransmission = "Összes";
    [ObservableProperty] private string selectedBranch = "Összes";
    [ObservableProperty] private bool showOnlyAvailable;
    [ObservableProperty] private bool isFilterVisible;

    // Oldal betöltésekor hívjuk
    [RelayCommand]
    public async Task LoadDataAsync()
    {
        IsLoading = true;
        var carsTask = _api.GetCarsAsync();
        var categoriesTask = _api.GetCategoriesAsync();
        var fuelTask = _api.GetFuelTypesAsync();
        var transmissionTask = _api.GetTransmissionTypesAsync();
        var branchTask = _api.GetBranchesAsync();

        await Task.WhenAll(carsTask, categoriesTask, fuelTask, transmissionTask, branchTask);

        _allCars = await carsTask;

        // Szűrő listák feltöltése
        Categories = new ObservableCollection<string>(
            new[] { "Összes" }.Concat((await categoriesTask).Select(x => x.Name)));
        FuelTypes = new ObservableCollection<string>(
            new[] { "Összes" }.Concat((await fuelTask).Select(x => x.Name)));
        TransmissionTypes = new ObservableCollection<string>(
            new[] { "Összes" }.Concat((await transmissionTask).Select(x => x.Name)));
        Branches = new ObservableCollection<string>(
            new[] { "Összes" }.Concat((await branchTask).Select(x => x.City)));

        IsLoading = false;
        ApplyFilters();
    }

    // Szűrők bármely változásakor meghívódik
    partial void OnSearchTextChanged(string value) => ApplyFilters();
    partial void OnSelectedCategoryChanged(string value) => ApplyFilters();
    partial void OnSelectedFuelTypeChanged(string value) => ApplyFilters();
    partial void OnSelectedTransmissionChanged(string value) => ApplyFilters();
    partial void OnSelectedBranchChanged(string value) => ApplyFilters();
    partial void OnShowOnlyAvailableChanged(bool value) => ApplyFilters();

    private void ApplyFilters()
    {
        var filtered = _allCars.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
            filtered = filtered.Where(c =>
                c.Brand.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                c.Model.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        if (SelectedCategory != "Összes")
            filtered = filtered.Where(c => c.CarCategory?.Name == SelectedCategory);

        if (SelectedFuelType != "Összes")
            filtered = filtered.Where(c => c.FuelType?.Name == SelectedFuelType);

        if (SelectedTransmission != "Összes")
            filtered = filtered.Where(c => c.TransmissionType?.Name == SelectedTransmission);

        if (SelectedBranch != "Összes")
            filtered = filtered.Where(c => c.Branch?.City == SelectedBranch);

        if (ShowOnlyAvailable)
            filtered = filtered.Where(c => c.Availability);

        Cars = new ObservableCollection<Car>(filtered);
        IsEmpty = Cars.Count == 0;
    }

    [RelayCommand]
    private void ToggleFilter() => IsFilterVisible = !IsFilterVisible;

    [RelayCommand]
    private void ResetFilters()
    {
        SelectedCategory = "Összes";
        SelectedFuelType = "Összes";
        SelectedTransmission = "Összes";
        SelectedBranch = "Összes";
        ShowOnlyAvailable = false;
        SearchText = string.Empty;
    }

    [RelayCommand]
    private async Task SelectCarAsync(Car car)
    {
        await Shell.Current.GoToAsync(nameof(CarDetailPage),
            new Dictionary<string, object> { ["Car"] = car });
    }

    [RelayCommand]
    private void Logout()
    {
        _auth.Logout();

        // DI konténerből kérjük a LoginPage-et, ne new-val példányosítsuk
        var loginPage = IPlatformApplication.Current!.Services.GetRequiredService<Pages.LoginPage>();
        Application.Current!.MainPage = new NavigationPage(loginPage);
    }
}