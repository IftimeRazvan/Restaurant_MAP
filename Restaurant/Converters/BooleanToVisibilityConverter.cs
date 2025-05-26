using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace Restaurant.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Verificăm dacă valoarea este de tip bool
            if (value is bool booleanValue)
            {
                // Dacă valoarea e true → Visibility.Visible
                return booleanValue ? Visibility.Visible : Visibility.Collapsed;
            }

            // În caz de eroare, ascunde elementul
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Nu avem nevoie de convertire inversă în acest caz
            throw new NotImplementedException();
        }
    }
}
