using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xamarin;
using Xamarin.Forms;

namespace Scanner.Models
{
    public class Image
    {
        public ImageSource ImageSource { get; private set; }
        public byte[] Data { get; private set; }
        public int Id { get; set; }

        public Image(byte[] data, int id)
        {
            Data = data;
            ImageSource = ImageSource.FromStream(() => new MemoryStream(data));
            Id = id;
        }
    }
}
