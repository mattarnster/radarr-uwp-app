using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RadarrApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
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

            //ImageBrush myBrush = new ImageBrush();
            //Image image = new Image();
            //image.Source = new BitmapImage(new Uri(movie.Images[1].Url));
            //myBrush.ImageSource = image.Source;
            //MovieInfo.Background = myBrush;
            //MovieInfo.BackgroundSizing = BackgroundSizing.OuterBorderEdge;
        }

        public string GetMovieDownloadedStatus()
        {
            return (movie.Downloaded ? "Downloaded" : "Not downloaded");
        }
    }
}
