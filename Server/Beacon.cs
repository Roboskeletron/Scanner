using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server
{
    static class Beacon
    {
        public static string DeviceName { get; private set; } = Dns.GetHostName();
        public static string ScanerName { get; set; }
        public static int Port { get; set; } = 8000;
        public static int Timeout { get; set; } = 500;
        public static byte[] Datagram
        {
            get
            {
                return datagram;
            }
            private set
            {
                FormDtgram();
            }
        }

        

        private static UdpClient udpClient = new UdpClient();
        private static byte[] datagram;
        private static bool IsRunning = false;

        private static Task sendTask = new Task(async delegate
        {
            while (IsRunning)
            {
                udpClient.Send(Datagram, Datagram.Length);
                await Task.Delay(Timeout);
            }
        });

        public static void Start(int port, int timeout)
        {
            if (Datagram == null)
                FormDtgram();
            udpClient.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.255"), port));
            IsRunning = true;
            if (timeout >= 100)
                Timeout = timeout;
            sendTask.Start();
        }

        public static void Stop()
        {
            IsRunning = false;
        }
        
        private static void FormDtgram()
        {
            byte[] devicename = Encoding.UTF8.GetBytes(DeviceName + "/n"), scanername = Encoding.UTF8.GetBytes(ScanerName + "/n"), port = Encoding.UTF8.GetBytes(Port.ToString());
            datagram = devicename.Concat(scanername.Concat(port)).ToArray();
        }
    }
}
