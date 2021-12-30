using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;

namespace Server.Services
{
    static class Server
    {
        public static int Port {
            get
            {
                return port;
            }
            set
            {
                port = value;
                udpClient = new UdpClient(port);
            }
        }
        public static bool IsRunning { get; private set; } = false;
        public static Queue<UdpReceiveResult> queue = new Queue<UdpReceiveResult>();
        public static int BufferSize { get; set; } = 4096;

        private static UdpClient udpClient = new UdpClient(port);
        private static int port = 7777;

        private static Random random = new Random();

        private static async void Receive()
        {
            while (IsRunning)
            {
                queue.Enqueue(await udpClient.ReceiveAsync());
                await Task.Run(Response);
            }
        }

        private static async void Response()
        {
            while (IsRunning)
            {
                if (queue.Count == 0)
                    break;
                var resp = queue.Dequeue();
                var data = await Scanner.Scan();
                byte[] imageSize = Encoding.UTF8.GetBytes(data.Length.ToString());
                udpClient.Send(imageSize, imageSize.Length, resp.RemoteEndPoint);
                await SendData(resp, new MemoryStream(data));
            }
        }

        private static async Task SendData(UdpReceiveResult resp, MemoryStream dataStream)
        {
            TcpClient tcpClient = new TcpClient();

            string req = Encoding.UTF8.GetString(resp.Buffer);
            int port = Convert.ToInt32(req.Split()[1]);
            await tcpClient.ConnectAsync(resp.RemoteEndPoint.Address, port);

            var netStream = tcpClient.GetStream();
            long left_bytes = dataStream.Length;
            do
            {
                byte[] data = new byte[BufferSize];
                int count = await dataStream.ReadAsync(data, 0, BufferSize);
                await netStream.WriteAsync(data, 0, count);
                left_bytes -= count;
            } while (left_bytes > 0);

            await netStream.FlushAsync();
            netStream.Close();
            tcpClient.Close();
        }

        public async static void Start()
        {
            if (IsRunning)
                return;
            IsRunning = true;
            await Task.Run(Receive);
        }

        public static void Stop()
        {
            if (!IsRunning)
                return;
            IsRunning = false;
        }
    }
}
