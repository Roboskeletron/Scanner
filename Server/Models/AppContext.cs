using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server.Services;

namespace Server.Models
{
    public class AppContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        public enum State { Online, Offline, NoDevice }

        public State CurrentState { get; private set; }

        public AppContext()
        {
            ContextMenuStrip menu = CreateContextMenu();

            trayIcon = new NotifyIcon()
            {
                Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("Server.Resources.HomeServer.ico")),
                ContextMenuStrip = menu,
                Visible = true,
                Text = "Server"
            };

            CurrentState = State.NoDevice;
            Initialize(menu.Items[0] as ToolStripMenuItem);
        }

        private async void Initialize(ToolStripMenuItem item)
        {
            Beacon.Port = Services.Server.Port;
            try
            {
                Beacon.ScanerName = await Scanner.GetScaner();
            }
            catch
            {
                MessageBox.Show("No scanning device found.");
                item.Text = "No scanner found";
            }

            if (Beacon.ScanerName != null)
            {
                SetOnline();
                item.Text = "Online";
            }
        }

        private void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;

            Application.Exit();
        }

        private void ChangeState(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            switch (CurrentState)
            {
                case State.Online:
                    SetOffline();
                    item.Text = "Offline";
                    break;
                case State.Offline:
                    SetOnline();
                    item.Text = "Online";
                    break;
                case State.NoDevice:
                    Initialize(item);
                    break;
            }
        }

        private void SetOffline()
        {
            Beacon.Stop();
            Services.Server.Stop();
            CurrentState = State.Offline;
        }

        private void SetOnline()
        {
            Beacon.Start(7778, 1000);
            Services.Server.Start();
            CurrentState = State.Online;
        }

        private ContextMenuStrip CreateContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem scanner = new ToolStripMenuItem("Scanner", null, ChangeState, "Choose scanner");
            menu.Items.Add(scanner);
            ToolStripMenuItem quit = new ToolStripMenuItem("Quit", null, Exit, "quit");
            menu.Items.Add(quit);
            
            return menu;
        }
    }
}
