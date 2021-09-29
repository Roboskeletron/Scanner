using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Scanner.Models
{
    class Scanner
    {
        public string ScannerName { get; set; }
        public string DeviceName { get; set; }
        public IPEndPoint IPEndPoint { get; private set; }
        public int LifeTime { get; private set; }
        public event EventHandler LifeTimeEnded;

        private bool Countdown = false;

        public Scanner(UdpReceiveResult raw_data, int life_time, out Scanner result)
        {
            try
            {
                result = this;

                IPEndPoint = raw_data.RemoteEndPoint;

                string[] info = Encoding.UTF8.GetString(raw_data.Buffer).Split(new string[] { "/n" }, StringSplitOptions.None);
                DeviceName = info[0];
                ScannerName = info[1];
                IPEndPoint.Port = Convert.ToInt32(info[2]);
                LifeTime = life_time;
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong raw data");
                result = null;
            }
        }

        public void ResetLife_time(int time)
        {
            LifeTime = time;
        }

        public async void StartLifeCountdown(int count)
        {
            if (Countdown)
                return;
            Countdown = true;
            while (Countdown)
            {
                await Task.Delay(count);
                LifeTime -= count;
                if (LifeTime <= 0)
                {
                    LifeTimeEnded(this, null);
                    return;
                }
            }
        }

        public void StopLifeCoutdown()
        {
            Countdown = false;
        }
    }
}
