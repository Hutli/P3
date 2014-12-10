using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using WebAPI;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ObservableCollection<Track> _list = new ObservableCollection<Track>();

        public ObservableCollection<Track> List { get { return _list; } set { _list = value; } }

        public Track NowPlaying;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(3)); // update from server every second
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        GetResults("localhost");
                        Console.WriteLine("trying \n");
                    }));
                }
            });
        }

        async public void GetResults(string ip)
        {
            ObservableCollection<Track> returnValue = new ObservableCollection<Track>();
            Track returnValue1 = new Track();
            try
            {
                var json = await GetPlaylist(ip);
                returnValue = (ObservableCollection<Track>)JsonConvert.DeserializeObject(json, typeof(ObservableCollection<Track>));

                var json1 = await GetNowPlaying(ip);
                NowPlaying = (Track)JsonConvert.DeserializeObject(json1, typeof(Track));
                NowPlayingImage.Source = new BitmapImage(new Uri(NowPlaying.Album.Images[0].URL));

                List.Clear();
                if(returnValue != null)
                    foreach (Track track in returnValue)
                    {
                        
                        List.Add(track);
                        Console.WriteLine("{0}", track.Name);
                    }
            }
            catch (Exception ex)
            {
            }
        }

        public static async Task<String> MakeRequest(Uri request, TimeSpan timeout) {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = timeout;
                    //Else Windows Phone will cache and not make new request to the server
                    client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now;

                    using (HttpResponseMessage response = await client.GetAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var str = await content.ReadAsStringAsync();
                                return str;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Http error");
                return null;
            }
        }

        public async Task<string> GetPlaylist(string ip)
        {
            UriBuilder uriBuilder = new UriBuilder("http", ip, 5555, "playlist");

            return await MakeRequest(uriBuilder.Uri, new TimeSpan(0, 0, 10));
        }


        public async Task<string> GetNowPlaying(string ip) //Vi kunne have genbrugt session fra app
        {
            UriBuilder uriBuilder = new UriBuilder("http", ip, 5555, "nowplaying");

            return await MakeRequest(uriBuilder.Uri, new TimeSpan(0, 0, 10));
        }

        private void LeftToRightMarquee()
        {
            double height = canMain.ActualHeight - tbmarquee.ActualHeight;
            tbmarquee.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -tbmarquee.ActualWidth;
            doubleAnimation.To = canMain.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(10));
            tbmarquee.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
        }
    } 
}
