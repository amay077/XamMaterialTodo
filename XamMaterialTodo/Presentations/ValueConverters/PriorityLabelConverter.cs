using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamMaterialTodo.Presentations.ValueConverters
{
    public sealed class PriorityLabelConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var priority = (int)value;
            switch (priority)
            {
                case 1: return "Low";
                case 2: return "Mid";
                case 3: return "High";
                default: return "";
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
