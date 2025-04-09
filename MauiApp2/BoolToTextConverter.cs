/*/*using System;
using Microsoft.Maui.Controls;
using System.Globalization;

namespace MauiApp2
{
    public class BoolToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Usuń z ulubionych" : "Dodaj do ulubionych";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null; // Nie potrzebujemy tej funkcji
        }
    }
}
/*/
