using System.Reflection;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RadarrApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

        public MainPage()
        {
            this.InitializeComponent();

            if (roamingSettings.Values["serverURL"] == null || (string) roamingSettings.Values["serverURL"] == ""
                || roamingSettings.Values["apiKey"].ToString().Length != 32)
            {
                roamingSettings.Values["https"] = false;
               
                foreach (NavigationViewItem navItem in NavView.MenuItems)
                {
                    navItem.IsEnabled = false;
                }
                NavigateToView("FirstRun", NavView);
            }
            else
            {
                NavigateToView("Movies", NavView);
            }
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                NavigateToView("Settings", sender);
            } 
            else
            {
                NavigationViewItem item = (NavigationViewItem) args.SelectedItem;
                NavigateToView((string)item.Content, sender);
            }
        }


        private bool NavigateToView(string clickedView, NavigationView nav)
        {
            var view = Assembly.GetExecutingAssembly()
                .GetType($"RadarrApp.Views.{clickedView}");

            if (string.IsNullOrWhiteSpace(clickedView) || view == null)
            {
                return false;
            }

            contentFrame.Navigate(view, null);

            return true;
        }

        private void NavView_BackRequested(NavigationView sender,
                                   NavigationViewBackRequestedEventArgs args)
        {
            On_BackRequested();
        }

        private bool On_BackRequested()
        {
            if (!contentFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed.
            if (NavView.IsPaneOpen &&
                (NavView.DisplayMode == NavigationViewDisplayMode.Compact ||
                 NavView.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            contentFrame.GoBack();
            return true;
        }

        public void On_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = contentFrame.CanGoBack;
        }


    }
}
