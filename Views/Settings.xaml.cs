using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RadarrApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

        public Settings()
        {
            this.InitializeComponent();

            HostnameValue.Text = (string) roamingSettings.Values["serverURL"] ?? "";
            APIKeyValue.Text = (string) roamingSettings.Values["apiKey"] ?? "";
            PortValue.Text = (string) roamingSettings.Values["port"] ?? "7878";
            UseHTTPS.IsOn = (bool) roamingSettings.Values["https"] == true ? true : false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int errors = 0;

            if (APIKeyValue.Text == "")
            {
                APIKeyValue.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                SavedText.Visibility = Visibility.Visible;
                SavedText.Text = "Please enter your Radarr API key.";
                SavedText.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                return;
            }

            roamingSettings.Values["serverURL"] = HostnameValue.Text;
            roamingSettings.Values["apiKey"] = APIKeyValue.Text;
            roamingSettings.Values["https"] = UseHTTPS.IsOn;
           
            try
            {
                int.Parse(PortValue.Text);
                roamingSettings.Values["port"] = PortValue.Text;
            } catch (Exception)
            {
                errors += 1;
            }

            if (errors > 0)
            {
                PortValue.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                SavedText.Visibility = Visibility.Visible;
                SavedText.Text = "Please enter a number in the port field.";
                SavedText.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            } else
            {
                PortValue.BorderBrush = new SolidColorBrush(Windows.UI.Colors.DarkGray);
                SavedText.Visibility = Visibility.Visible;
                SavedText.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
                SavedText.Text = "Your settings have been saved.";
            }

        }
    }
}
