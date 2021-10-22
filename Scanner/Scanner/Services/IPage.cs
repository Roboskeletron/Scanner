using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Scanner.Services
{
    public interface IPage
    {
        void GetScannerPage(Scanner.ViewModels.ScannerPageViewModel model);
    }
}
