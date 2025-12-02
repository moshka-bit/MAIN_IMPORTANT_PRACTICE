using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace MAIN_IMPORTANT_PRACTICE;

public class DateOnlyToDateTimeOffsetConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateOnly dateOnly)
        {
            DateTime dateTime = dateOnly.ToDateTime(TimeOnly.MinValue);
            return new DateTimeOffset(dateTime);
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTimeOffset dateTimeOffset)
        {
            DateTime dateTime = dateTimeOffset.DateTime;
            return DateOnly.FromDateTime(dateTime);
        }

        return DateOnly.MinValue;
    }
}