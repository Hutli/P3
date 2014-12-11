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

        private ObservableCollection<Track> _playlist = new ObservableCollection<Track>();
        public ObservableCollection<Track> Playlist { get { return _playlist; } set { _playlist = value; } }

        private ObservableCollection<Track> _history = new ObservableCollection<Track>();
        public ObservableCollection<Track> History { get { return _history; } set { _history = value; } }

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
                        GetResults("77.68.200.85");
                        Console.WriteLine("trying \n");
                    }));
                }
            });
        }

        async public void GetResults(string ip)
        {
            ObservableCollection<Track> playlistReturn = new ObservableCollection<Track>();
            ObservableCollection<Track> historyReturn = new ObservableCollection<Track>();

            try
            {
                var playlistJson = await GetPlaylist(ip);
                playlistReturn = (ObservableCollection<Track>)JsonConvert.DeserializeObject(playlistJson, typeof(ObservableCollection<Track>));

                var historyJson = await GetHistory(ip);
                historyReturn = (ObservableCollection<Track>)JsonConvert.DeserializeObject(historyJson, typeof(ObservableCollection<Track>));

                var nowPlayingJson = await GetNowPlaying(ip);
                if ((Track)JsonConvert.DeserializeObject(nowPlayingJson, typeof(Track)) != null && NowPlaying != (Track)JsonConvert.DeserializeObject(nowPlayingJson, typeof(Track))) {
                    NowPlaying = (Track)JsonConvert.DeserializeObject(nowPlayingJson, typeof(Track));
                    NowPlayingImage.Source = new BitmapImage(new Uri(NowPlaying.Album.Images[1].URL));
                    tbmarquee.Text = NowPlaying.ToString();
                    Progress.Maximum = NowPlaying.Duration;
                    Progress.Value = NowPlaying.CurrentDurationStep;
                    if(NowPlaying.Name != tbmarquee.Text)
                        LeftToRightMarquee();
                }

                Playlist.Clear();
                if(playlistReturn != null)
                    foreach (Track track in playlistReturn)
                    {
                        Playlist.Add(track);
                    }

                History.Clear();
                if (historyReturn != null)
                    foreach (Track track in historyReturn)
                    {
                        History.Add(track);
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

        public async Task<string> GetHistory(string ip)
        {
            UriBuilder uriBuilder = new UriBuilder("http", ip, 5555, "history");

            return await MakeRequest(uriBuilder.Uri, new TimeSpan(0, 0, 10));
        }

        private void LeftToRightMarquee()
        {
            double height = canMain.ActualHeight - tbmarquee.ActualHeight;
            tbmarquee.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = tbmarquee.ActualWidth;
            doubleAnimation.To = -canMain.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(30));
            tbmarquee.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
        }
    } 
}
