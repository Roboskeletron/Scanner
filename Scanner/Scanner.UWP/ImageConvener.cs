using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Scanner.UWP
{
    public class ImageConvener : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            byte[] source = (byte[])value;
            BitmapImage image = new BitmapImage();
            InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
            stream.WriteAsync(source.AsBuffer());
            stream.Seek(0);
            image.SetSource(stream);
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
