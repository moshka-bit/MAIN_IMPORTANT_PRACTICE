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
            // Преобразуем DateOnly в DateTime, затем в DateTimeOffset
            DateTime dateTime = dateOnly.ToDateTime(TimeOnly.MinValue);
            return new DateTimeOffset(dateTime);
        }

        return null; // Возвращаем null для пустого значения
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTimeOffset dateTimeOffset)
        {
            // Преобразуем DateTimeOffset в DateTime, затем в DateOnly
            DateTime dateTime = dateTimeOffset.DateTime;
            return DateOnly.FromDateTime(dateTime);
        }

        return DateOnly.MinValue;
    }
}