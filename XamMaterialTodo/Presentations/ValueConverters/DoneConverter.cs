using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamMaterialTodo.Presentations.ValueConverters
{
    public sealed class DoneConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isDone = (bool)value;
            return isDone ?
                "https://raw.githubusercontent.com/amay077/XamMaterialTodo/master/img/outline_check_box_black_48dp.png" :
                "https://raw.githubusercontent.com/amay077/XamMaterialTodo/master/img/outline_check_box_outline_blank_black_48dp.png";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
