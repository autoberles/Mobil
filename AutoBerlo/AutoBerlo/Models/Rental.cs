using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AutoBerlo.Models;

public class Rental
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int UserId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string? Damage { get; set; }
    public int? DamageCost { get; set; }
    public int FullPrice { get; set; }
    public Car? Car { get; set; }

    public int Days => StartDate.HasValue && EndDate.HasValue
        ? (EndDate.Value.Date - StartDate.Value.Date).Days + 1
        : 0;
}

public class CreateRentalRequest
{
    [JsonPropertyName("car_id")]
    public int CarId { get; set; }

    [JsonPropertyName("start_date")]
    public string StartDate { get; set; } = string.Empty;

    [JsonPropertyName("end_date")]
    public string EndDate { get; set; } = string.Empty;
}