using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Scanner.ViewModels;

namespace Scanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectionPage : ContentPage
    {
        private ConnectionPageViewModel viewModel = new ConnectionPageViewModel();

        public ConnectionPage()
        {
            InitializeComponent();

            BindingContext = viewModel;
            viewModel.Scanners.CollectionChanged += Scanners_CollectionChanged;

            Services.BeaconUpdate.CollectionUpdate += Handle_ItemUpdate;
            Services.BeaconUpdate.Port = 7778;
            Services.BeaconUpdate.StartUpdate();
        }

        private void Scanners_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (viewModel.Scanners.Count > 0)
                ScannersView.EndRefresh();
            else ScannersView.BeginRefresh();
        }

        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            Models.Scanner scanner = (Models.Scanner)e.Item;

            ScannerPageViewModel scannerPageViewModel = new ScannerPageViewModel(scanner.DeviceName, scanner.ScannerName, scanner.IPEndPoint);

            ((ListView)sender).SelectedItem = null;

            if (Device.RuntimePlatform == Device.UWP)
            {
                DependencyService.Get<Services.IPage>().GetScannerPage(scannerPageViewModel);
            }
            else
            {
                await Navigation.PushAsync(new ScannerPage(scannerPageViewModel));
            }
        }

        private void Handle_ItemUpdate(object sender, EventArgs e)
        {
            System.Net.Sockets.UdpReceiveResult data = (System.Net.Sockets.UdpReceiveResult)sender;
            viewModel.Update(data, 500);
        }
    }
}
