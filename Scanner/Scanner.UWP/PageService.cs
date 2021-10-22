using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

[assembly:Xamarin.Forms.Dependency(typeof(Scanner.UWP.PageService))]
namespace Scanner.UWP
{
    class PageService : Services.IPage
    {
        public static ViewModels.ScannerPageViewModel viewModel = null;

        public void GetScannerPage(ViewModels.ScannerPageViewModel model)
        {
            viewModel = model;
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(ScannerView));
        }
    }
}
