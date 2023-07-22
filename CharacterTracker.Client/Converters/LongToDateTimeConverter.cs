using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace CharacterTracker.Client.Converters
{
    public class LongToDateTimeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (!(value is long timestamp))
                return null;

            return new DateTime(timestamp).ToString("G");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
