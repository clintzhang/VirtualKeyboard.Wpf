using System;
using System.Globalization;
using System.Windows.Data;
using VirtualKeyboard.Wpf.Core.Types;
using VirtualKeyboard.Wpf.Core.Views;

namespace VirtualKeyboard.Wpf.Core.Converters {
    internal class KeyboardTypeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var type = (KeyboardType)value;
            switch (type) {
                case KeyboardType.Alphabet: return new AlphabetView();
                case KeyboardType.Special: return new SpecialCharactersView();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}