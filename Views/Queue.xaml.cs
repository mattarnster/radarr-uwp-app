using RadarrApp.Services;
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
    /// Queue page, lists the items in the radarr queue
    /// </summary>
    public sealed partial class Queue : Page
    {
        readonly RadarrSharp.RadarrClient radarrClient;
        private IList<RadarrSharp.Models.Queue> movies;
        public Queue()
        {
            this.InitializeComponent();
            RadarrService radarrService = new RadarrService();

            radarrClient = radarrService.GetService();

            GetQueue();
        }

        private async void GetQueue()
        {
            try
            {
                movies = await radarrClient.Queue.GetQueue();
                progressRing.IsActive = false;

                if (movies.Count == 0)
                {
                    noQueueItems.Visibility = Visibility.Visible;
                }
                else
                {
                    ContentGridView.ItemsSource = movies;
                }
            } catch
            {
                sorryMessage.Visibility = Visibility.Visible;
                progressRing.IsActive = false;
            }
        }
    }
}
