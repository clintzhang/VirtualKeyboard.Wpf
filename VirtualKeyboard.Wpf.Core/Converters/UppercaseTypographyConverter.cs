﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VirtualKeyboard.Wpf.Core.Converters;

internal class UppercaseTypographyConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        var uppercase = (bool)value;
        if (uppercase) return FontCapitals.AllSmallCaps;
        return FontCapitals.Normal;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}