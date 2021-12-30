using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Server.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            NavView.SelectedItem = NavView.MenuItems.First();
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Type page = null;
            switch (args.SelectedItemContainer.Tag.ToString())
            {
                case "scanner":
                    page = typeof(Scanner);
                    break;
                case "server":
                    page = typeof(Server);
                    break;
                default:
                    return;
            }
            FrameNavigationOptions options = new FrameNavigationOptions() { TransitionInfoOverride = args.RecommendedNavigationTransitionInfo };
            contentFrame.NavigateToType(page, null, options);
        }
    }
}
