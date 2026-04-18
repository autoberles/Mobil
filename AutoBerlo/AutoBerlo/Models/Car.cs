using System.Text.Json.Serialization;

namespace AutoBerlo.Models;

public class Car
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("availability")]
    public bool Availability { get; set; }

    [JsonPropertyName("brand")]
    public string Brand { get; set; } = string.Empty;

    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;

    [JsonPropertyName("own_weight")]
    public int OwnWeight { get; set; }

    [JsonPropertyName("total_weight")]
    public int TotalWeight { get; set; }

    [JsonPropertyName("number_of_seats")]
    public int NumberOfSeats { get; set; }

    [JsonPropertyName("number_of_doors")]
    public int NumberOfDoors { get; set; }

    [JsonPropertyName("price")]
    public int Price { get; set; }

    [JsonPropertyName("license_plate")]
    public string LicensePlate { get; set; } = string.Empty;

    [JsonPropertyName("mileage")]
    public int Mileage { get; set; }

    [JsonPropertyName("luggage_capacity")]
    public int LuggageCapacity { get; set; }

    [JsonPropertyName("cubic_capacity")]
    public int CubicCapacity { get; set; }

    [JsonPropertyName("tank_capacity")]
    public int? TankCapacity { get; set; }

    [JsonPropertyName("battery_capacity")]
    public int? BatteryCapacity { get; set; }

    [JsonPropertyName("performance_kw")]
    public int PerformanceKw { get; set; }

    [JsonPropertyName("performance_hp")]
    public int PerformanceHp { get; set; }

    [JsonPropertyName("last_service_date")]
    public DateTime LastServiceDate { get; set; }

    [JsonPropertyName("inspection_expiry_date")]
    public DateTime InspectionExpiryDate { get; set; }

    [JsonPropertyName("default_price_per_day")]
    public int DefaultPricePerDay { get; set; }

    [JsonPropertyName("img_url")]
    public string ImgUrl { get; set; } = string.Empty;

    [JsonPropertyName("branch")]
    public Branch? Branch { get; set; }

    [JsonPropertyName("fuel_type")]
    public FuelType? FuelType { get; set; }

    [JsonPropertyName("transmission_type")]
    public TransmissionType? TransmissionType { get; set; }

    [JsonPropertyName("car_category")]
    public CarCategory? CarCategory { get; set; }

    [JsonPropertyName("wheel_drive_type")]
    public WheelDriveType? WheelDriveType { get; set; }

    [JsonPropertyName("additional_equipment")]
    public AdditionalEquipment? AdditionalEquipment { get; set; }

    [JsonPropertyName("branch_id")]
    public int BranchId { get; set; }

    [JsonPropertyName("transmission_id")]
    public int TransmissionId { get; set; }

    [JsonPropertyName("fuel_type_id")]
    public int FuelTypeId { get; set; }

    [JsonPropertyName("wheel_drive_type_id")]
    public int WheelDriveTypeId { get; set; }

    [JsonPropertyName("car_category_id")]
    public int CarCategoryId { get; set; }

    public string DisplayName => $"{Brand} {Model}";
    public string PriceDisplay => $"{DefaultPricePerDay:N0} Ft/nap";
    public string AvailabilityText => Availability ? "Elérhető" : "Foglalt";
}

public class Branch
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;
}

public class FuelType
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class TransmissionType
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class CarCategory
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class WheelDriveType
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class AdditionalEquipment
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("car_id")]
    public int CarId { get; set; }

    [JsonPropertyName("parking_sensors")]
    public bool ParkingSensors { get; set; }

    [JsonPropertyName("air_conditioning_id")]
    public int AirConditioningId { get; set; }

    [JsonPropertyName("air_conditioning")]
    public AirConditioning? AirConditioning { get; set; }

    [JsonPropertyName("heated_seats")]
    public bool HeatedSeats { get; set; }

    [JsonPropertyName("navigation")]
    public bool Navigation { get; set; }

    [JsonPropertyName("leather_seats")]
    public bool LeatherSeats { get; set; }

    [JsonPropertyName("tempomat")]
    public bool Tempomat { get; set; }
}

public class AirConditioning
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}