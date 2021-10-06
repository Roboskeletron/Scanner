using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scanner.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannerPage : ContentPage
    {
        private ScannerPageViewModel viewModel = null;

        public ScannerPage(ScannerPageViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            await Task.Delay(10);
            BindingContext = viewModel;
        }

        private void Add_Clicked(object sender, EventArgs e)
        {
            viewModel.ScannImage();
        }
    }
}