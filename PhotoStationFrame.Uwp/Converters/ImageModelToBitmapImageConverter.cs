using PhotoStationFrame.Uwp.ViewObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace PhotoStationFrame.Uwp.Converters
{
    public class ImageModelToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            var imageModel = value as ImageModel;
            if (imageModel == null)
            {
                return DependencyProperty.UnsetValue;
            }
            var bitmap = new BitmapImage();

            Task.Run(async () =>
            {
                try
                {


                    using (var stream = await imageModel.Client.GetThumbnailData(imageModel.Url))
                    using (var memStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memStream);
                        memStream.Position = 0;
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                        // Your UI update code goes here!
                        bitmap.SetSource(memStream.AsRandomAccessStream());

                        });
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            });

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException("Two way binding not supported.");
        }
    }
}
