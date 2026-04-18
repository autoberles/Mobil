namespace AutoBerlo.Models;

public class CarDisplayItem
{
    public Car Car { get; set; } = null!;

    public string FuelTypeName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string TransmissionName { get; set; } = string.Empty;
    public string BranchCity { get; set; } = string.Empty;

    public string DisplayName => Car.DisplayName;
    public string PriceDisplay => Car.PriceDisplay;
    public string AvailabilityText => Car.AvailabilityText;
    public bool Availability => Car.Availability;
    public string ImgUrl => Car.ImgUrl;
    public int Year => Car.Year;
    public int NumberOfSeats => Car.NumberOfSeats;
    public int Id => Car.Id;
}