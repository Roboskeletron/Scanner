using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Windows.Input;
using Server.Services;
using Server.Models;

namespace Server.ViewModels
{
    public class ScannerViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<Image> Images { get; set; } = new ObservableCollection<Image>();
        public string ScannerName { get => scanner_name;
            private set
            {
                if (scanner_name != value)
                {
                    scanner_name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ScannerName"));
                }
            }
        }
        public bool CanSaveOrDelete { get; set; } = false;
        public Image CurrentImage
        {
            get => currentImage;
            set
            {
                if (currentImage != value)
                {
                    currentImage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentImage"));
                }
            }
        }

        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private Image currentImage = null;
        private int image_id = -1;
        private string scanner_name;

        public ScannerViewModel()
        {
            Images.CollectionChanged += Images_CollectionChanged;

            DeleteCommand = new Command(execute: () =>
            {
                int index = Images.IndexOf(CurrentImage);
                Images.Remove(CurrentImage);
                IEnumerable<Models.Image> images = Images.TakeWhile(image => image.Id < CurrentImage.Id);
                CurrentImage = images.Count() > 0 ? images.Last() : Images.Count > 0 ? Images.First() : null;
            },
            canExecute: () =>
            {
                return CanSaveOrDelete;
            });

            SaveCommand = new Command(execute: async () =>
            {
                FileSavePicker picker = new FileSavePicker();
                picker.FileTypeChoices.Add("PDF document", new List<string> { ".pdf" });
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                picker.SuggestedFileName = "New document";

                var file = await picker.PickSaveFileAsync();

                if (file != null)
                {
                    CachedFileManager.DeferUpdates(file);
                    var stream = Pdf.CreatePdf(Images);
                    await FileIO.WriteBytesAsync(file, stream);
                    await CachedFileManager.CompleteUpdatesAsync(file);
                }
            },
            canExecute: () =>
            {
                return CanSaveOrDelete;
            });

            AddCommand = new Command(execute: async () =>
            {
                var data = await Scanning.Scan();

                Images.Add(new Image(data, ++image_id));
                CurrentImage = Images.Last();
            },
            canExecute: () =>
            {
                return true;
            });
            Init();
        }

        private async void Init()
        {
            ScannerName = await Scanning.GetScanner();
        }

        private void Images_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CanSaveOrDelete = Images.Count > 0;
            (DeleteCommand as Command).RaiseCanExecuteChanged();
            (SaveCommand as Command).RaiseCanExecuteChanged();
        }

        //private async void ScannImage()
        //{
        //    var data = await Scanning.Scan();

        //    Images.Add(new Image(data, ++image_id));
        //    CurrentImage = Images.Last();
        //}
    }
}
