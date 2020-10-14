using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using RadarrApp.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RadarrApp.Views
{
    /// <summary>
    /// Movies page, lists all movies in the radarr library
    /// </summary>
    public sealed partial class Movies : Page
    {
        RadarrSharp.RadarrClient radarrClient;
        RadarrService radarrService = new RadarrService();
        private IList<RadarrSharp.Models.Movie> movies;

        public Movies()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.DataContext = this;

            radarrClient = radarrService.GetService();

            GetMovies();
        }

        private async void GetMovies()
        {
            movies = await radarrClient.Movie.GetMovies();
            foreach (var movie in movies)
            {
                foreach (var image in movie.Images)
                {
                    image.Url = (radarrClient.UseSsl ? "https://" : "http://") + radarrClient.Host + ":" + radarrClient.Port + "/api" + image.Url + "?apikey=" + radarrClient.ApiKey;
                }
            }

            progressRing.IsActive = false;
            ContentGridView.ItemsSource = movies;
        }

        //public string GetMoviePosterImage(RadarrSharp.Models.Movie movie)
        //{
           
        //}

        private void ContentGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            RadarrSharp.Models.Movie clickedMovie = (RadarrSharp.Models.Movie)e.ClickedItem;
            Frame.Navigate(typeof(RadarrApp.Views.Movie), clickedMovie, new EntranceNavigationTransitionInfo());
        }

        private void Btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            progressRing.IsActive = true;
            GetMovies();
        }
    }
}
