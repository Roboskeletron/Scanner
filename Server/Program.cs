using System;
using System.Windows.Forms;

namespace Server
{
    static class Program
    {
        [MTAThread]
        static void Main()
        {
            Application.Run(new Models.AppContext());
        }
    }
}