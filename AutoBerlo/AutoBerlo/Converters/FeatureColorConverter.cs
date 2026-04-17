using System.Globalization;

namespace AutoBerlo.Converters;

// true → fehér szöveg, false → szürke szöveg az extra felszereltség listánál
public class FeatureColorConverter : IValueConverter
{
    public static readonly FeatureColorConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
            return b
                ? Color.FromArgb("#FFFFFF")
                : Color.FromArgb("#555555");
        return Color.FromArgb("#555555");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}