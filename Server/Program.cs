using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Server
{
    static class Program
    {
        public static bool IsRun { get; set; } = true;

        [STAThread]
        static void Main()
        {
            Application.Run(new AppContext());
            //Beacon.ScanerName = Scaner.GetScaner();
            ////Beacon.ScanerName = "some scanner";
            //Server.Port = 7777;
            //Beacon.Port = Server.Port;
            //Beacon.Start(7778, 1000);
            //Server.Start();
            //while (IsRun)
            //{
            //    string input = Console.ReadLine();
            //    if (input == "exit")
            //        break;
            //}
        }
    }

    public class AppContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        public AppContext()
        {
            trayIcon = new NotifyIcon()
            {
                Icon = new System.Drawing.Icon(SystemIcons.WinLogo, 40, 40),
                ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Exit", Exit)
            }),
                Visible = true
            };

            Beacon.ScanerName = Scanner.GetScaner();
            //Beacon.ScanerName = "some scanner";
            Server.Port = 7777;
            Beacon.Port = Server.Port;
            Beacon.Start(7778, 1000);
            Server.Start();
        }

        void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;

            Application.Exit();
        }
    }
}