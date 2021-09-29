using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

[assembly: Dependency(typeof(Scanner.Droid.FileSaver))]
namespace Scanner.Droid
{
    public class FileSaver : Services.IFileSystem
    {
        public static byte[] PdfData = null;

        public void SavePdf(byte[] data)
        {
            PdfData = data;
            Intent intent = new Intent(Forms.Context, typeof(FileSaverActivity));
            Forms.Context.StartActivity(intent);
        }
    }
}