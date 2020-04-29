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
    public sealed partial class Status : Page
    {
        ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
        RadarrSharp.RadarrClient radarrClient;
        RadarrSharp.Models.SystemStatus systemStatus;
        public Status()
        {
            this.InitializeComponent();
            radarrClient = new RadarrSharp.RadarrClient((string)roamingSettings.Values["serverURL"], 7878, (string)roamingSettings.Values["apiKey"]);
            GetStatusDetails();
        }

        private async void GetStatusDetails()
        {
            systemStatus = await radarrClient.SystemStatus.GetSystemStatus();
            progressRing.IsActive = false;
            Bindings.Update();
        }
    }
}
