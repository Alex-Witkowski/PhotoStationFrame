using PhotoStationFrame.Uwp.ViewObjects;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace PhotoStationFrame.Uwp.Converters
{
    public class ImageModelToStretchConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var imageModel = value as ImageModel;
            if (imageModel == null)
            {
                return DependencyProperty.UnsetValue;
            }

            return imageModel.Width > imageModel.Height ? Stretch.UniformToFill : Stretch.Uniform;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException("Two way binsing not supported");
        }
    }
}
