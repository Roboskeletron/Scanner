using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using Windows.Storage;
using Xamarin.Forms;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

[assembly:Dependency(typeof(Scanner.UWP.FileSaver))]
namespace Scanner.UWP
{
    public class FileSaver : Services.IFileSystem
    {
        public static byte[] PdfData;

        public void SavePdf(byte[] data)
        {
            PdfData = data;
            _ = CoreApplication.CreateNewView().DispatcherQueue.TryEnqueue(() =>
            {
                // This code runs on the new thread
                Windows.UI.Xaml.Controls.Frame frame = new Windows.UI.Xaml.Controls.Frame();
                frame.Navigate(typeof(SaverPage), data);
                Window.Current.Content = frame;
                Window.Current.Activate();
            });
        }
    }
}
