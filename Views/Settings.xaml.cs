using RadarrApp.Services;
using RadarrSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
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
#pragma warning disable IDE0044 // Add readonly modifier
        private ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
#pragma warning restore IDE0044 // Add readonly modifier

        public Settings()
        {
            this.InitializeComponent();

            HostnameValue.Text = (string) roamingSettings.Values["serverURL"] ?? "";
            APIKeyValue.Text = (string) roamingSettings.Values["apiKey"] ?? "";
            PortValue.Text = (string) roamingSettings.Values["port"] ?? "";
            
            if ((bool)roamingSettings.Values["https"])
            {
                UseHTTPS.IsOn = true;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            int errors = 0;

            if (APIKeyValue.Text == "" || APIKeyValue.Text.Length != 32)
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
            } 
            catch (Exception)
            {
                errors += 1;
            }

            if (errors > 0)
            {
                PortValue.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                SavedText.Visibility = Visibility.Visible;
                SavedText.Text = "Please enter a number in the port field.";
                SavedText.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            } 
            else
            {
                try
                {
                    RadarrService rs = new RadarrService();
                    RadarrClient rc = rs.GetService();

                    await rc.Movie.GetMovies();
                } catch
                {
                    var dialog = new MessageDialog("There was an error testing your settings, please ensure the details entered are correct.", "Error testing connection");
                    _ = await dialog.ShowAsync();
                    return;
                }

                PortValue.BorderBrush = new SolidColorBrush(Windows.UI.Colors.DarkGray);
                APIKeyValue.BorderBrush = new SolidColorBrush(Windows.UI.Colors.DarkGray);
                SavedText.Visibility = Visibility.Visible;
                SavedText.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
                SavedText.Text = "Your settings have been saved.";

                Debug.WriteLine(Frame.Parent.GetType().FullName);

                NavigationView navView = (NavigationView)Frame.Parent;

                foreach (NavigationViewItem navItem in navView.MenuItems)
                {
                    navItem.IsEnabled = true;
                }

            }

        }

        private void UseHTTPS_Toggled(object sender, RoutedEventArgs e)
        {
            var toggleSwitch = (ToggleSwitch)sender;
            if (toggleSwitch.IsOn)
            {
                PortValue.Text = "443";
                PortValue.IsEnabled = false;
            }
            else
            {
                PortValue.Text = "";
                PortValue.IsEnabled = false;
            }
            
        }
    }
}
