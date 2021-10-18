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

        public async void SavePdf(byte[] data)
        {
            FileSavePicker picker = new FileSavePicker();
            picker.FileTypeChoices.Add("PDF document", new List<string> { ".pdf" });
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.SuggestedFileName = "New document";

            StorageFile file = await picker.PickSaveFileAsync();

            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                await FileIO.WriteBytesAsync(file, data);
                await CachedFileManager.CompleteUpdatesAsync(file);
            }
        }
    }
}
