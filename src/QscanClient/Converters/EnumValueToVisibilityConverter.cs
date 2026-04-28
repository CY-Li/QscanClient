using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace QscanClient.Converters;

public class EnumValueToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return Visibility.Collapsed;

        string? checkValue = value.ToString();
        string? targetValue = parameter.ToString();

        if (checkValue == null || targetValue == null)
            return Visibility.Collapsed;

        bool equal = checkValue.Equals(targetValue, StringComparison.OrdinalIgnoreCase);
        return equal ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
