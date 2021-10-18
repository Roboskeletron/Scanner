using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Scanner.Services;

namespace Scanner.ViewModels
{
    public class ScannerPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Models.Image> Images { get; set; } = new ObservableCollection<Models.Image>();
        public string DeviceName { get; private set; }
        public string ScannerName { get; private set; }
        public bool CanSaveOrDelete { get; set; } = false;
        public Models.Image CurrentImage
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
        public event PropertyChangedEventHandler PropertyChanged;

        private System.Net.IPEndPoint endPoint;
        private Models.Image currentImage = null;
        private int image_id = -1;

        public ScannerPageViewModel(string device_name, string scanner_name, System.Net.IPEndPoint endPoint)
        {
            DeviceName = device_name;
            ScannerName = scanner_name;
            this.endPoint = endPoint;
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
                IFileSystem fileSystem = DependencyService.Get<IFileSystem>();
                byte[] pdf = null;
                await Task.Run(() =>
                {
                    pdf = PdfCreator.CreatePdf(Images);
                });
                fileSystem.SavePdf(pdf);
            },
            canExecute: () =>
            {
                return CanSaveOrDelete;
            });
        }

        private void Images_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CanSaveOrDelete = Images.Count > 0;
            (DeleteCommand as Command).ChangeCanExecute();
            (SaveCommand as Command).ChangeCanExecute();
        }

        public async void ScannImage()
        {
            var data = await Services.Scanning.ScannImage(endPoint);

            Images.Add(new Models.Image(data, ++image_id));
            CurrentImage = Images.Last();
        }
    }
}
