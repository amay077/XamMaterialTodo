using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamMaterialTodo.ValueConverters
{
    public sealed class PriorityColorConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var priority = (int)value;
            switch (priority)
            {
                case 1: return Color.LightGreen;
                case 2: return Color.Blue;
                case 3: return Color.Red;
                default: return Color.Transparent;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
