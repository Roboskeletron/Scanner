using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server.Services
{
    public static class AppDataManager
    {
        public static DirectoryInfo DataDir
        {
            get
            { 
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Scanning server");
                if (Directory.Exists(path))
                    return new DirectoryInfo(path);
                else return Directory.CreateDirectory(path);
            }
            private set
            {

            }
        }

        public const string DefaultScannerIdName = "Default scanner.data";

        public static string GetDefaultScannerId()
        {
            FileInfo[] files = DataDir.GetFiles();
            int index = Array.FindIndex(files, item => item.Name == "Default scanner.data");

            if (index == -1)
                return null;

            string id = null;

            using (StreamReader reader = files[index].OpenText())
                id = reader.ReadToEnd();

            return id;
        }

        public static void SetDefaultScannerId(string id)
        {
            FileInfo[] files = DataDir.GetFiles();
            int index = Array.FindIndex(files, item => item.Name == DefaultScannerIdName);

            using (var stream = index==-1 ? File.Create(Path.Combine(DataDir.FullName, DefaultScannerIdName)) : files[index].OpenWrite())
            {
                byte[] data = Encoding.UTF8.GetBytes(id);
                stream.Write(data, 0, data.Length);
            }
        }
    }
}
