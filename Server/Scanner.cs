using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIA;

namespace Server
{
    static class Scanner
    {
        private static Device MyScanner;

        public static bool IsConnected { get; private set; } = false;
        public static bool IsBusy { get; private set; } = false;
        public static string ScannerName { get; private set; }
        public static string DeviceID { get; private set; }

        public const string wiaFormatJPEG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";


        public static string GetScaner()
        {
            if (!IsConnected)
            {
                MyScanner = null;
                do
                {
                    ICommonDialog dialog = new CommonDialog();
                    MyScanner = dialog.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, true, false);
                } while (MyScanner == null);

                ScannerName = MyScanner.Properties["Name"].get_Value().ToString();
                //ScannerName = MyScanner.Properties["Name"].get_Value().ToString();
                DeviceID = MyScanner.DeviceID;
                IsConnected = true;
            }
            return ScannerName;
        }

        public static bool SetScaner(string deviceId)
        {
            DeviceManager deviceManager = new DeviceManager();
            for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
            {
                if (deviceManager.DeviceInfos[i].DeviceID == deviceId)
                {
                    MyScanner = deviceManager.DeviceInfos[i].Connect();
                    IsConnected = true;
                    DeviceID = deviceId;
                    return true;
                }
            }
            return false;
        }

        public static void FreeScaner()
        {
            if (!IsConnected)
                return;
            while (IsBusy) { }
            MyScanner = null;
            ScannerName = null;
            DeviceID = null;
            IsConnected=false;
        }

        public async static Task<byte[]> Scan()
        {
            while (IsBusy) { }
            IsBusy = true;
            ImageFile data = null;
            await Task.Run(delegate
            {
                data = (ImageFile)MyScanner.Items[1].Transfer(wiaFormatJPEG);
            });
            IsBusy = false;
            return (byte[])data.FileData.get_BinaryData();
        }
    }
}
