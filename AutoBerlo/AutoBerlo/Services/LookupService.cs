using AutoBerlo.Models;
using System.Text.Json;

namespace AutoBerlo.Services;

public class LookupService
{
    private readonly HttpClient _http;
    private readonly AuthService _auth;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public LookupService(HttpClient http, AuthService auth)
    {
        _http = http;
        _auth = auth;
    }

    public List<FuelType> FuelTypes { get; private set; } = [];
    public List<TransmissionType> TransmissionTypes { get; private set; } = [];
    public List<CarCategory> CarCategories { get; private set; } = [];
    public List<WheelDriveType> WheelDriveTypes { get; private set; } = [];
    public List<Branch> Branches { get; private set; } = [];
    public List<AirConditioning> AirConditioningTypes { get; private set; } = [];

    public bool IsLoaded { get; private set; }

    public async Task LoadAllAsync()
    {
        if (IsLoaded) return;
        _auth.SetAuthHeader(_http);

        try
        {
            var tasks = new[]
            {
                LoadAsync<FuelType>("api/fuel_types"),
                LoadAsync<TransmissionType>("api/transmission_types"),
                LoadAsync<CarCategory>("api/car_categories"),
                LoadAsync<WheelDriveType>("api/wheel_drive_types"),
                LoadAsync<Branch>("api/branches"),
                LoadAsync<AirConditioning>("api/air_conditioning_types")
            };

            var results = await Task.WhenAll(tasks);

            FuelTypes = results[0].Cast<FuelType>().ToList();
            TransmissionTypes = results[1].Cast<TransmissionType>().ToList();
            CarCategories = results[2].Cast<CarCategory>().ToList();
            WheelDriveTypes = results[3].Cast<WheelDriveType>().ToList();
            Branches = results[4].Cast<Branch>().ToList();
            AirConditioningTypes = results[5].Cast<AirConditioning>().ToList();

            IsLoaded = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Lookup hiba: {ex.Message}");
        }
    }

    public string GetFuelTypeName(int id) =>
        FuelTypes.FirstOrDefault(x => x.Id == id)?.Name ?? "—";

    public string GetTransmissionName(int id) =>
        TransmissionTypes.FirstOrDefault(x => x.Id == id)?.Name ?? "—";

    public string GetCategoryName(int id) =>
        CarCategories.FirstOrDefault(x => x.Id == id)?.Name ?? "—";

    public string GetWheelDriveName(int id) =>
        WheelDriveTypes.FirstOrDefault(x => x.Id == id)?.Name ?? "—";

    public string GetBranchCity(int id) =>
        Branches.FirstOrDefault(x => x.Id == id)?.City ?? "—";

    public string GetBranchAddress(int id) =>
        Branches.FirstOrDefault(x => x.Id == id)?.Address ?? "—";

    public string GetBranchPhone(int id) =>
        Branches.FirstOrDefault(x => x.Id == id)?.PhoneNumber ?? "—";

    public string GetAirConditioningName(int id) =>
        AirConditioningTypes.FirstOrDefault(x => x.Id == id)?.Name ?? "—";

    private async Task<IList<object>> LoadAsync<T>(string url) where T : class
    {
        try
        {
            var resp = await _http.GetAsync(url);
            var body = await resp.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<T>>(body, JsonOpts);
            return list?.Cast<object>().ToList() ?? [];
        }
        catch { return []; }
    }
}