using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace RadarrApp.Services
{
    class RadarrService
    {
        ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
        public RadarrSharp.RadarrClient GetService()
        {
            var radarrClient = new RadarrSharp.RadarrClient(
                (string)roamingSettings.Values["serverURL"],
                int.Parse((string)roamingSettings.Values["port"]),
                (string)roamingSettings.Values["apiKey"], null, (bool)roamingSettings.Values["https"]);

            return radarrClient;
        }
    }
}
