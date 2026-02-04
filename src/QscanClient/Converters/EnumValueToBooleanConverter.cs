using System;
using System.Globalization;
using System.Windows.Data;

namespace QscanClient.Converters;

public class EnumValueToBooleanConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        string? checkValue = value.ToString();
        string? targetValue = parameter.ToString();

        if (checkValue == null || targetValue == null)
            return false;

        return checkValue.Equals(targetValue, StringComparison.OrdinalIgnoreCase);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b && b)
        {
            return parameter?.ToString() ?? Binding.DoNothing;
        }

        return Binding.DoNothing;
    }
}
