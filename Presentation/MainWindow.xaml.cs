using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using WebAPI;

namespace Presentation
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Track> _history = new ObservableCollection<Track>();
        private ObservableCollection<Track> _playlist = new ObservableCollection<Track>();
        private Track _nowPlaying;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            Task.Run(async () =>
                           {
                               while(true)
                               {
                                   await Task.Delay(TimeSpan.FromSeconds(3)); // update from server every second

                                   var serverData = await GetResults(Ip);

                                   Application.Current.Dispatcher.Invoke(() =>
                                                                         {
                                                                             ApplyChanges(serverData);
                                                                             Console.WriteLine("trying \n");
                                                                         });
                               }
                           });

            Task.Run(async () =>
                           {
                               while(true)
                               {
                                   await Task.Delay(TimeSpan.FromMilliseconds(100)); // update from server every second
                                   Application.Current.Dispatcher.Invoke(() => { Progress.Value += TimeSpan.FromMilliseconds(100).TotalMilliseconds; });
                               }
                           });
        }

        private ObservableCollection<Track> Playlist
        {
            get { return _playlist; }
            set { _playlist = value; }
        }

        private ObservableCollection<Track> History
        {
            get { return _history; }
            set { _history = value; }
        }

        private string Ip { get; set; }

        private async Task<ServerData> GetResults(string ip)
        {
            var serverData = new ServerData();
            try
            {
                var playlistJson = await GetPlaylist(ip);
                serverData.Playlist = (ObservableCollection<Track>)JsonConvert.DeserializeObject(playlistJson, typeof(ObservableCollection<Track>));

                var historyJson = await GetHistory(ip);
                serverData.History = (ObservableCollection<Track>)JsonConvert.DeserializeObject(historyJson, typeof(ObservableCollection<Track>));

                var nowPlayingJson = await GetNowPlaying(ip);
                serverData.NowPlaying = (Track)JsonConvert.DeserializeObject(nowPlayingJson, typeof(Track));
            } catch(Exception) {}
            return serverData;
        }

        private void ApplyChanges(ServerData serverData)
        {
            ToStringColumn.Width = PlaylistListView.ActualWidth - ImageColumn.ActualWidth - RankColumn.ActualWidth - TotalScoreColumn.ActualWidth - ThumbsUpColumn.ActualWidth - 10;
            ToStringColumnHist.Width = ToStringColumn.Width;

            Playlist.Clear();
            if(serverData.Playlist != null)
            {
                foreach(var track in serverData.Playlist)
                    Playlist.Add(track);
            }

            History.Clear();
            if(serverData.History != null)
            {
                foreach(var track in serverData.History)
                {
                    History.Add(track);
                    HistoryListView.ScrollIntoView(track);
                }
            }

            if(serverData.NowPlaying != null && !Equals(_nowPlaying, serverData.NowPlaying))
            {
                _nowPlaying = serverData.NowPlaying;
                Progress.Value = _nowPlaying.CurrentDurationStep;
                if(string.IsNullOrEmpty(LeftTextBoxMarquee.Text) || !_nowPlaying.ToString().Equals(LeftTextBoxMarquee.Text))
                {
                    NowPlayingImage.Source = new BitmapImage(new Uri(_nowPlaying.Album.Images[1].Url));

                    LeftTextBoxMarquee.Text = _nowPlaying.ToString();
                    RightTextBoxMarquee.Text = _nowPlaying.ToString();
                    Progress.Maximum = _nowPlaying.Duration;
                    RightToLeftMarquee();
                }
            }
        }

        private static async Task<String> MakeRequest(Uri request, TimeSpan timeout)
        {
            try
            {
                using(var client = new HttpClient())
                {
                    client.Timeout = timeout;
                    //Else Windows Phone will cache and not make new request to the server
                    client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now;

                    using(var response = await client.GetAsync(request))
                    {
                        if(response.IsSuccessStatusCode)
                        {
                            using(var content = response.Content)
                            {
                                var str = await content.ReadAsStringAsync();
                                return str;
                            }
                        }
                        return null;
                    }
                }
            } catch(Exception)
            {
                Console.WriteLine("Http error");
                return null;
            }
        }

        private async Task<string> GetPlaylist(string ip)
        {
            var uriBuilder = new UriBuilder("http", ip, 5555, "playlist");

            return await MakeRequest(uriBuilder.Uri, new TimeSpan(0, 0, 10));
        }

        private async Task<string> GetNowPlaying(string ip)
        {
            var uriBuilder = new UriBuilder("http", ip, 5555, "nowplaying");

            return await MakeRequest(uriBuilder.Uri, new TimeSpan(0, 0, 10));
        }

        private async Task<string> GetHistory(string ip)
        {
            var uriBuilder = new UriBuilder("http", ip, 5555, "history");

            return await MakeRequest(uriBuilder.Uri, new TimeSpan(0, 0, 10));
        }

        private async void RightToLeftMarquee()
        {
            var height = CanMain.ActualHeight - LeftTextBoxMarquee.ActualHeight;
            LeftTextBoxMarquee.Margin = new Thickness(0, height / 2, 0, 0);
            RightTextBoxMarquee.Margin = new Thickness(0, height / 2, 0, 0);

            var textSize = MeasureString(LeftTextBoxMarquee.Text, LeftTextBoxMarquee);

            var maxWidth = Math.Max(CanMain.ActualWidth, textSize.Width);
            if(Equals(maxWidth, textSize.Width))
                maxWidth = maxWidth * 2 - (maxWidth - CanMain.ActualWidth);

            var leftDoubleAnimation = new DoubleAnimation();
            var rightDoubleAnimation = new DoubleAnimation();

            leftDoubleAnimation.From = 0;
            leftDoubleAnimation.To = -maxWidth;
            leftDoubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            leftDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(20));

            rightDoubleAnimation.From = maxWidth;
            rightDoubleAnimation.To = 0;
            rightDoubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            rightDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(20));

            LeftTextBoxMarquee.BeginAnimation(Canvas.LeftProperty, null);
            RightTextBoxMarquee.BeginAnimation(Canvas.LeftProperty, null);

            await Task.Delay(TimeSpan.FromSeconds(2));

            LeftTextBoxMarquee.BeginAnimation(Canvas.LeftProperty, leftDoubleAnimation);
            RightTextBoxMarquee.BeginAnimation(Canvas.LeftProperty, rightDoubleAnimation);
        }

        private Size MeasureString(string candidate, TextBlock inputTextBlock)
        {
            var formattedText = new FormattedText(candidate, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(inputTextBlock.FontFamily, inputTextBlock.FontStyle, inputTextBlock.FontWeight, inputTextBlock.FontStretch), inputTextBlock.FontSize, Brushes.Black);

            return new Size(formattedText.Width, formattedText.Height);
        }

        private struct ServerData
        {
            public ObservableCollection<Track> History;
            public Track NowPlaying;
            public ObservableCollection<Track> Playlist;
        }
    }
}