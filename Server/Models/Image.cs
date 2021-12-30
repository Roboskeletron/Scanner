using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

namespace Server.Models
{
    public class Image
    {
        public BitmapImage ImageSource { get; private set; } = new BitmapImage();
        public IRandomAccessStream Stream { get; private set; }
        public int Id { get; set; }

        public Image(IRandomAccessStream stream, int id)
        {
            Stream = stream;
            ImageSource.SetSource(stream);
            Id = id;
        }
    }
}
