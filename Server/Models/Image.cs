using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using System.IO;

namespace Server.Models
{
    public class Image
    {
        public BitmapImage ImageSource { get; private set; } = new BitmapImage();
        public byte[] Data { get; private set; }
        public int Id { get; set; }

        public Image(IRandomAccessStream stream, int id)
        {
            ImageSource.SetSource(stream);
            var _stream = stream.AsStream();
            Data = new byte[_stream.Length];
            _stream.Position = 0;
            _stream.Read(Data, 0, Data.Length);
            Id = id;
        }
    }
}
