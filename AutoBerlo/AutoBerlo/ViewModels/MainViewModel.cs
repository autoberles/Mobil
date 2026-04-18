using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoBerlo.Services;
using AutoBerlo.Models;
using AutoBerlo.Pages;
using System.Collections.ObjectModel;

namespace AutoBerlo.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ApiService _api;
    private readonly AuthService _auth;
    private readonly LookupService _lookup;
    private readonly IServiceProvider _services;
    private List<Car> _allCars = [];

    public MainViewModel(ApiService api, AuthService auth,
                         LookupService lookup, IServiceProvider services)
    {
        _api = api;
        _auth = auth;
        _lookup = lookup;
        _services = services;
    }

    [ObservableProperty] private ObservableCollection<CarDisplayItem> cars = [];
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private bool isEmpty;
    [ObservableProperty] private string searchText = string.Empty;

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

    [RelayCommand]
    public async Task LoadDataAsync()
    {
        IsLoading = true;

        await _lookup.LoadAllAsync();
        _allCars = await _api.GetCarsAsync();

        Categories = new ObservableCollection<string>(
            new[] { "Összes" }.Concat(_lookup.CarCategories.Select(x => x.Name)));
        FuelTypes = new ObservableCollection<string>(
            new[] { "Összes" }.Concat(_lookup.FuelTypes.Select(x => x.Name)));
        TransmissionTypes = new ObservableCollection<string>(
            new[] { "Összes" }.Concat(_lookup.TransmissionTypes.Select(x => x.Name)));
        Branches = new ObservableCollection<string>(
            new[] { "Összes" }.Concat(_lookup.Branches.Select(x => x.City)));

        SelectedCategory = "Összes";
        SelectedFuelType = "Összes";
        SelectedTransmission = "Összes";
        SelectedBranch = "Összes";

        IsLoading = false;
        ApplyFilters();
    }

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
            filtered = filtered.Where(c =>
                _lookup.GetCategoryName(c.CarCategoryId) == SelectedCategory);

        if (SelectedFuelType != "Összes")
            filtered = filtered.Where(c =>
                _lookup.GetFuelTypeName(c.FuelTypeId) == SelectedFuelType);

        if (SelectedTransmission != "Összes")
            filtered = filtered.Where(c =>
                _lookup.GetTransmissionName(c.TransmissionId) == SelectedTransmission);

        if (SelectedBranch != "Összes")
            filtered = filtered.Where(c =>
                _lookup.GetBranchCity(c.BranchId) == SelectedBranch);

        if (ShowOnlyAvailable)
            filtered = filtered.Where(c => c.Availability);

        Cars = new ObservableCollection<CarDisplayItem>(
            filtered.Select(c => new CarDisplayItem
            {
                Car = c,
                FuelTypeName = _lookup.GetFuelTypeName(c.FuelTypeId),
                CategoryName = _lookup.GetCategoryName(c.CarCategoryId),
                TransmissionName = _lookup.GetTransmissionName(c.TransmissionId),
                BranchCity = _lookup.GetBranchCity(c.BranchId)
            }));

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
    private async Task SelectCarAsync(CarDisplayItem item)
    {
        await Shell.Current.GoToAsync(nameof(CarDetailPage),
            new Dictionary<string, object> { ["CarId"] = item.Id });
    }

    [RelayCommand]
    private void Logout()
    {
        _auth.Logout();
        var loginPage = _services.GetRequiredService<Pages.LoginPage>();
        Application.Current!.MainPage = new NavigationPage(loginPage);
    }
}