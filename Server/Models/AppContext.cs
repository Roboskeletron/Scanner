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
            Initialize();
        }

        private void Initialize()
        {
            Beacon.Port = Services.Server.Port;
            Beacon.ScanerName = Scanner.GetScaner();
            Beacon.Start(7778, 1000);
            Services.Server.Start();
        }

        void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;

            Application.Exit();
        }
        
        private ContextMenuStrip CreateContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item = new ToolStripMenuItem("Quit", null, Exit, "quit");
            menu.Items.Add(item);
            return menu;
        }
    }
}
