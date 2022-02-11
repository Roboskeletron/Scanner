using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Scanner.Services
{
    static class Scanning
    {
        public static int BufferSize { get; set; } = 4096;
        public static int Port { get; set; } = 8012;

        public static async Task<byte[]> ScannImage(IPEndPoint endPoint)
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(endPoint);
            TcpListener tcpListener = new TcpListener(Port);
            tcpListener.Start();

            byte[] req = Encoding.UTF8.GetBytes("SR " + Port.ToString());
            await udpClient.SendAsync(req, req.Length);
            int byteCount = 0;
            await Task.Run(() =>
            {
                byteCount = Convert.ToInt32(Encoding.UTF8.GetString(udpClient.Receive(ref endPoint)));
            });
            var client = await tcpListener.AcceptTcpClientAsync();

            MemoryStream dataStream = new MemoryStream();
            NetworkStream networkStream = client.GetStream();
            do
            {
                byte[] data = new byte[BufferSize];

                int count = await networkStream.ReadAsync(data, 0, data.Length);

                await dataStream.WriteAsync(data, 0, count);
                byteCount -= count;
            } while (byteCount > 0);
            
            await dataStream.FlushAsync();
            networkStream.Close();
            client.Close();
            tcpListener.Stop();

            return dataStream.ToArray();
        }
    }
}
