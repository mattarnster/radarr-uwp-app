using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Linq;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RadarrApp.Views
{
    /// <summary>
    /// Movie, singular movie page used when an item is clicked on the Movie page.
    /// </summary>
    public sealed partial class Movie : Page
    {
        public RadarrSharp.Models.Movie movie;

        public Movie()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            movie = (RadarrSharp.Models.Movie)e.Parameter;
        }

        public string GetMovieDownloadedStatus()
        {
            return (movie.Downloaded ? "Downloaded" : "Not downloaded");
        }

        public BitmapImage GetMoviePosterImage()
        {
            BitmapImage bitmapImage = new BitmapImage();
               
            if (movie.Images.Count > 0)
            {
                var PosterPath = from RadarrSharp.Models.Image i in movie.Images where (i.CoverType == RadarrSharp.Enums.CoverType.Poster) select i.Url;
                string url = PosterPath.Single();
                bitmapImage.UriSource = new Uri(url);

                return bitmapImage;

            }

            return null;
        }

        public BitmapImage GetMovieBackgroundImage()
        {
            BitmapImage bitmapImage = new BitmapImage();

            if (movie.Images.Count > 0)
            {
                var PosterPath = from RadarrSharp.Models.Image i in movie.Images where (i.CoverType == RadarrSharp.Enums.CoverType.Banner || i.CoverType == RadarrSharp.Enums.CoverType.FanArt) select i.Url;
                string url = PosterPath.Single();
                bitmapImage.UriSource = new Uri(url);

                return bitmapImage;

            }

            return null;
        }
    }
}
