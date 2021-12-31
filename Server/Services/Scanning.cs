using System;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;
using Windows.Devices.Scanners;

namespace Server.Services
{
    public static class Scanning
    {
        public static ImageScanner Scanner { get; private set; }

        public static async Task<IRandomAccessStream> Scan()
        {
            InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
            var result = await Scanner.ScanPreviewToStreamAsync(ImageScannerScanSource.Flatbed, stream);
            if (!result.Succeeded)
                throw new Exception("Scanning faild");
            return stream;
        }

        public static async Task<string> GetScanner()
        {
            DevicePicker picker = new DevicePicker();
            picker.Filter.SupportedDeviceClasses.Clear();
            picker.Filter.SupportedDeviceClasses.Add(DeviceClass.ImageScanner);
            var device = await picker.PickSingleDeviceAsync(new Windows.Foundation.Rect());
            if (device == null)
                return null;
            Scanner = await ImageScanner.FromIdAsync(device.Id);
            Scanner.FlatbedConfiguration.Format = ImageScannerFormat.Jpeg;
            Scanner.FlatbedConfiguration.DesiredResolution = new ImageScannerResolution() { DpiX = 1200, DpiY = 1200 };
            
            return device.Name;
        }
    }
}
