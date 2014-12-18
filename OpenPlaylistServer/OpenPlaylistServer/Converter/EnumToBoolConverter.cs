using System;
using System.Globalization;
using System.Windows.Data;

namespace OpenPlaylistServer.Converter {
    public class EnumToBoolConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return parameter.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return parameter;
        }
    }
}