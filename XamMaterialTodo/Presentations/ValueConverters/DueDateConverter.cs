using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamMaterialTodo.Presentations.ValueConverters
{
    public sealed class DueDateConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dt = (DateTimeOffset?)value;

            return dt != null ? "〜" + dt.Value.ToString("M/d") : string.Empty;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
