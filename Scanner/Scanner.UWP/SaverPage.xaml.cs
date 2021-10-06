using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using Windows.Storage;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Scanner.UWP
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SaverPage : Page
    {
        public SaverPage()
        {
            this.InitializeComponent();
            SavePdf();
        }

        private static async void SavePdf()
        {
            FileSavePicker picker = new FileSavePicker();
            picker.FileTypeChoices.Add("PDF document", new List<string> { ".pdf" });
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.SuggestedFileName = "New document";

            StorageFile file = await picker.PickSaveFileAsync();

            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                await FileIO.WriteBytesAsync(file, FileSaver.PdfData);
                await CachedFileManager.CompleteUpdatesAsync(file);
            }
        }
    }
}
