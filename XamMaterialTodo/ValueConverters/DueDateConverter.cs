using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamMaterialTodo.ValueConverters
{
    public sealed class DueDateConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dt = (DateTimeOffset?)value;
            return dt?.ToString("M/d") ?? string.Empty;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
