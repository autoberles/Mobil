using System.Globalization;

namespace AutoBerlo.Converters;

// true (elérhető) → zöld, false (foglalt) → piros háttér a badge-hez
public class AvailabilityColorConverter : IValueConverter
{
    public static readonly AvailabilityColorConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool available)
            return available
                ? Color.FromArgb("#1E3A1E")   // sötétzöld
                : Color.FromArgb("#3A1E1E");  // sötétpiros
        return Color.FromArgb("#333333");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}