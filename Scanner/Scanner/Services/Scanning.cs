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
        public static int BufferSize { get; set; } = 1024;

        private static UdpClient udpClient = new UdpClient();
        private static readonly string endMessage = "\nDataSent";

        public static async Task<byte[]> ScannImage(IPEndPoint endPoint)
        {
            udpClient.Connect(endPoint);
            byte[] req = Encoding.UTF8.GetBytes("Scann req");
            await udpClient.SendAsync(req, req.Length);

            MemoryStream dataStream = new MemoryStream();
            byte[] data = new byte[BufferSize];

            await Task.Run(delegate
            {
                bool isEnd = false;
                do
                {
                    data = udpClient.Receive(ref endPoint);
                    isEnd = Encoding.UTF8.GetString(data) != endMessage;
                    if (isEnd)
                        dataStream.Write(data, 0, data.Length);
                } while (isEnd);

                dataStream.Flush();
            });

            return dataStream.ToArray();
        }
    }
}
