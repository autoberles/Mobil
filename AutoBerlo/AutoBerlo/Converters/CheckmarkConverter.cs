using System.Globalization;

namespace AutoBerlo.Converters;

// true → "✓", false → "✗"
public class CheckmarkConverter : IValueConverter
{
    public static readonly CheckmarkConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && b ? "✓" : "✗";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}