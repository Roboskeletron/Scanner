using System;
using System.Net.Sockets;

namespace Scanner.Services
{
    static class BeaconUpdate
    {
        public static event EventHandler CollectionUpdate;
        public static bool IsRunning { get; private set; }
        public static int Port { get; set; }

        public async static void StartUpdate()
        {
            if (IsRunning)
                return;
            IsRunning = true;
            UdpClient udpClient = new UdpClient(Port);
            while (IsRunning)
            {
                UdpReceiveResult data = await udpClient.ReceiveAsync();
                CollectionUpdate(data, null);
            }
        }

        public static void StopUpdate()
        {
            IsRunning = false;
        }
    }
}
