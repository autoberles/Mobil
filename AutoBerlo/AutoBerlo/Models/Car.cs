using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBerlo.Models;

public class Car
{
    public int Id { get; set; }
    public bool Availability { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Color { get; set; } = string.Empty;
    public int OwnWeight { get; set; }
    public int TotalWeight { get; set; }
    public int NumberOfSeats { get; set; }
    public int NumberOfDoors { get; set; }
    public int Price { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public int LuggageCapacity { get; set; }
    public int CubicCapacity { get; set; }
    public int? TankCapacity { get; set; }
    public int? BatteryCapacity { get; set; }
    public int PerformanceKw { get; set; }
    public int PerformanceHp { get; set; }
    public DateTime LastServiceDate { get; set; }
    public DateTime InspectionExpiryDate { get; set; }
    public int DefaultPricePerDay { get; set; }
    public string ImgUrl { get; set; } = string.Empty;

    // Beágyazott objektumok
    public Branch? Branch { get; set; }
    public FuelType? FuelType { get; set; }
    public TransmissionType? TransmissionType { get; set; }
    public CarCategory? CarCategory { get; set; }
    public WheelDriveType? WheelDriveType { get; set; }
    public AdditionalEquipment? AdditionalEquipment { get; set; }

    // Segédmezők a UI-hoz
    public string DisplayName => $"{Brand} {Model}";
    public string PriceDisplay => $"{DefaultPricePerDay:N0} Ft/nap";
    public string AvailabilityText => Availability ? "Elérhető" : "Foglalt";
}

public class Branch
{
    public int Id { get; set; }
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class FuelType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TransmissionType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CarCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class WheelDriveType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class AdditionalEquipment
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public bool ParkingSensors { get; set; }
    public bool HeatedSeats { get; set; }
    public bool Navigation { get; set; }
    public bool LeatherSeats { get; set; }
    public bool Tempomat { get; set; }
    public AirConditioning? AirConditioning { get; set; }
}

public class AirConditioning
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}