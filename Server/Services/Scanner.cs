using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIA;

namespace Server.Services
{
    static class Scanner
    {
        private static Device MyScanner;

        public static bool IsConnected { get; private set; } = false;
        public static bool IsBusy { get; private set; } = false;
        public static string ScannerName { get; private set; }
        public static string DeviceID { get; private set; }

        public const string wiaFormatJPEG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";


        public async static Task<string> GetScaner()
        {
            string id = AppDataManager.GetDefaultScannerId();

            SetScaner(id);
            bool res = await GetFromDialog();

            if (!res)
                return null;
            else AppDataManager.SetDefaultScannerId(DeviceID);

            return ScannerName;
        }

        private static async Task<bool> GetFromDialog()
        {
            if (IsConnected)
                return true;

            MyScanner = null;

            await Task.Run(() =>
            {
                ICommonDialog dialog = new CommonDialog();
                MyScanner = dialog.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, true, false);
            });

            if (MyScanner == null)
                return false;

            ScannerName = MyScanner.Properties["Name"].get_Value().ToString();
            DeviceID = MyScanner.DeviceID;
            IsConnected = true;

            return true;
        }
        private static void SetScaner(string deviceId)
        {
            DeviceManager deviceManager = new DeviceManager();
            for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
            {
                if (deviceManager.DeviceInfos[i].DeviceID == deviceId)
                {
                    MyScanner = deviceManager.DeviceInfos[i].Connect();
                    IsConnected = true;
                    DeviceID = deviceId;
                    ScannerName = MyScanner.Properties["Name"].get_Value().ToString();
                    return;
                }
            }
        }

        public static void FreeScaner()
        {
            if (!IsConnected)
                return;
            while (IsBusy) { }
            MyScanner = null;
            ScannerName = null;
            DeviceID = null;
            IsConnected = false;
        }

        public static async Task<byte[]> Scan()
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
