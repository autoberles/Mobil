using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBerlo.Models;

public class UserProfile
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public string Role { get; set; } = string.Empty;

    public string FullName => $"{LastName} {FirstName}";
}