using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;
using System.Threading.Tasks;
using Windows.Devices.Scanners;
using Windows.Storage.Streams;

namespace Server.Services
{
    public class Scanner
    {
        private ImageScanner imageScanner;

        public Scanner(string deviceId)
        {
            GetScanner(deviceId);
        }

        private async void GetScanner(string id)
        {
            imageScanner = await ImageScanner.FromIdAsync(id);
            imageScanner.FlatbedConfiguration.Format = ImageScannerFormat.Jpeg;
        }

        public async Task<Stream> Scann()
        {
            InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
            var res = await imageScanner.ScanPreviewToStreamAsync(ImageScannerScanSource.Default, stream);
            if (!res.Succeeded)
                return null;
            return stream.AsStream();
        }
    }
}
