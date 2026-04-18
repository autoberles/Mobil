using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Json;
using System.Text.Json;
using AutoBerlo.Models;

namespace AutoBerlo.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly AuthService _auth;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = null
    };

    public ApiService(HttpClient http, AuthService auth)
    {
        _http = http;
        _auth = auth;
    }


    public async Task<(bool Success, string Message)> LoginAsync(LoginRequest req)
    {
        try
        {
            var json = JsonSerializer.Serialize(req, JsonOpts);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var resp = await _http.PostAsync("api/auth/login", content);
            var body = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
            {
                var data = JsonSerializer.Deserialize<LoginResponse>(body, JsonOpts);
                if (!string.IsNullOrEmpty(data?.Token))
                {
                    _auth.SaveToken(data.Token);
                    return (true, "Sikeres bejelentkezés!");
                }
            }
            return (false, ParseError(body));
        }
        catch (Exception ex)
        {
            return (false, $"Kapcsolódási hiba: {ex.Message}");
        }
    }

    public async Task<(bool Success, string Message)> RegisterAsync(RegisterRequest req)
    {
        try
        {
            var json = JsonSerializer.Serialize(req, JsonOpts);
            System.Diagnostics.Debug.WriteLine($"Register küldés: {json}");

            var content = new StringContent(
                json,
                System.Text.Encoding.UTF8,
                "application/json"
            );

            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json")
            {
                CharSet = "utf-8"
            };

            var resp = await _http.PostAsync("api/auth/register", content);
            var body = await resp.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"Register hiba: {body}");

            return resp.IsSuccessStatusCode
                ? (true, "Sikeres regisztráció! Ellenőrizd az emailed.")
                : (false, ParseError(body));
        }
        catch (Exception ex)
        {
            return (false, $"Kapcsolódási hiba: {ex.Message}");
        }
    }

    public async Task<(bool Success, string Message)> ForgotPasswordAsync(string email)
    {
        try
        {
            var json = JsonSerializer.Serialize(
                new ForgotPasswordRequest { Email = email }, JsonOpts);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var resp = await _http.PostAsync("api/auth/forgot_password", content);
            var body = await resp.Content.ReadAsStringAsync();
            return resp.IsSuccessStatusCode
                ? (true, "Ha az email létezik, elküldtük a kódot.")
                : (false, ParseError(body));
        }
        catch (Exception ex)
        {
            return (false, $"Kapcsolódási hiba: {ex.Message}");
        }
    }

    public async Task<(bool Success, string Message)> ResetPasswordAsync(ResetPasswordRequest req)
    {
        try
        {
            var json = JsonSerializer.Serialize(req, JsonOpts);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var resp = await _http.PostAsync("api/auth/reset_password", content);
            var body = await resp.Content.ReadAsStringAsync();
            return resp.IsSuccessStatusCode
                ? (true, "Jelszó sikeresen megváltoztatva!")
                : (false, ParseError(body));
        }
        catch (Exception ex)
        {
            return (false, $"Kapcsolódási hiba: {ex.Message}");
        }
    }

    public async Task<List<Car>> GetCarsAsync()
    {
        _auth.SetAuthHeader(_http);
        try
        {
            var response = await _http.GetAsync("api/cars");
            var body = await response.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine($"Cars JSON (első 1500 kar): {body[..Math.Min(1500, body.Length)]}");

            var cars = JsonSerializer.Deserialize<List<Car>>(body, JsonOpts);
            return cars ?? [];
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Cars hiba: {ex.Message}");
            return [];
        }
    }

    public async Task<Car?> GetCarByIdAsync(int id)
    {
        _auth.SetAuthHeader(_http);
        try
        {
            var response = await _http.GetAsync($"api/cars/{id}");
            var body = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"CarById JSON: {body[..Math.Min(2000, body.Length)]}");
            return JsonSerializer.Deserialize<Car>(body, JsonOpts);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"CarById hiba: {ex.Message}");
            return null;
        }
    }


    public async Task<List<CarCategory>> GetCategoriesAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<CarCategory>>("api/car_categories", JsonOpts) ?? [];
        }
        catch { return []; }
    }

    public async Task<List<FuelType>> GetFuelTypesAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<FuelType>>("api/fuel_types", JsonOpts) ?? [];
        }
        catch { return []; }
    }

    public async Task<List<TransmissionType>> GetTransmissionTypesAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<TransmissionType>>("api/transmission_types", JsonOpts) ?? [];
        }
        catch { return []; }
    }

    public async Task<List<Branch>> GetBranchesAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<Branch>>("api/branches", JsonOpts) ?? [];
        }
        catch { return []; }
    }


    public async Task<(bool Success, string Message, Rental? Rental)> CreateRentalAsync(
        CreateRentalRequest req)
    {
        _auth.SetAuthHeader(_http);
        try
        {
            var json = JsonSerializer.Serialize(req, JsonOpts);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var resp = await _http.PostAsync("api/rental", content);
            var body = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
            {
                using var doc = JsonDocument.Parse(body);
                var rentalEl = doc.RootElement.GetProperty("rental");
                var rental = JsonSerializer.Deserialize<Rental>(rentalEl.GetRawText(), JsonOpts);
                return (true, "Bérlés sikeresen létrehozva!", rental);
            }
            return (false, ParseError(body), null);
        }
        catch (Exception ex)
        {
            return (false, $"Kapcsolódási hiba: {ex.Message}", null);
        }
    }

    public async Task<List<Rental>> GetMyRentalsAsync()
    {
        _auth.SetAuthHeader(_http);
        try
        {
            return await _http.GetFromJsonAsync<List<Rental>>("api/rentals/my_rentals", JsonOpts) ?? [];
        }
        catch { return []; }
    }


    public async Task<UserProfile?> GetMyProfileAsync()
    {
        _auth.SetAuthHeader(_http);
        try
        {
            return await _http.GetFromJsonAsync<UserProfile>("api/users/me", JsonOpts);
        }
        catch { return null; }
    }


    private static string ParseError(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return "Ismeretlen hiba.";
        try
        {
            using var doc = JsonDocument.Parse(json);
            foreach (var key in new[] { "message", "title", "detail" })
                if (doc.RootElement.TryGetProperty(key, out var val))
                    return val.GetString() ?? "Ismeretlen hiba.";
            if (doc.RootElement.ValueKind == JsonValueKind.String)
                return doc.RootElement.GetString() ?? "Ismeretlen hiba.";
        }
        catch
        {
            return json.Trim('"');
        }
        return "Ismeretlen hiba.";
    }
}
