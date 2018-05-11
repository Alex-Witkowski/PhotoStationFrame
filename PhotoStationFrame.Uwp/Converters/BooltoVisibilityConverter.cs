using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PhotoStationFrame.Uwp.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(!(value is bool))
            {
                return DependencyProperty.UnsetValue;
            }

            var myValue = Invert ? !((bool)value) : (bool)value;
            return myValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException("Two way binding not supported.");
        }
    }
}
