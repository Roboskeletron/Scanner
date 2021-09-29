using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;

namespace Scanner.ViewModels
{
    class ConnectionPageViewModel
    {

        public ObservableCollection<Models.Scanner> Scanners { get; private set; } = new ObservableCollection<Models.Scanner>();

        public int LifeTime { get; set; } = 3000;

        public void Update(UdpReceiveResult data, int countdown)
        {
            new Models.Scanner(data, LifeTime, out Models.Scanner scanner);

            if (scanner == null)
                return;

            int index = FindIndex(scanner);
            if (index == -1)
            {
                scanner.LifeTimeEnded += Scanner_LifeTimeEnded;
                scanner.StartLifeCountdown(countdown);
                Scanners.Add(scanner);
            }
            else
            {
                Scanners[index].ResetLife_time(LifeTime);
            }
        }

        private void Scanner_LifeTimeEnded(object sender, EventArgs e)
        {
            Scanners.RemoveAt(FindIndex(sender as Models.Scanner));
        }

        private int FindIndex(Models.Scanner scanner)
        {
            for (int i = 0; i < Scanners.Count; i++)
            {
                if (Scanners[i].IPEndPoint.ToString() == scanner.IPEndPoint.ToString())
                    return i;
            }
            return -1;
        }
    }
}
