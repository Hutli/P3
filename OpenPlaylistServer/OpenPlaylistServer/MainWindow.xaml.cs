using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SpotifyDotNet;
using System.IO;
using NAudio;
using System.ComponentModel;
using Nancy.Hosting.Self;
using System.Windows.Threading;

namespace OpenPlaylistServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Spotify session = Spotify.Instance;
        private static byte[] appkey = OpenPlaylistServer.Properties.Resources.spotify_appkey;
        
        private static NAudio.Wave.WaveFormat activeFormat;
        private static NAudio.Wave.BufferedWaveProvider sampleStream;
        private static NAudio.Wave.WaveOut waveOut;

        private static Playlist pl = new Playlist();
        List<PTrack> history = new List<PTrack>(); 
        List<User> users = new List<User>();

        public static Action UpdateUIDelegate;

        public MainWindow(){
            InitializeComponent();

            var hostConfig = new HostConfiguration();
            hostConfig.UrlReservations.CreateAutomatically = true;

            var host = new NancyHost(hostConfig, new Uri("http://localhost:5555"));
            host.Start();

            session.OnLogInError += OnLogInError;
            session.OnLogInSuccess += OnLoginSuccess;
            session.MusicDelivery += OnRecieveData;
            session.TrackEnded += TrackEnded;
            //session.SearchComplete += (results) => SpotifyLoggedIn.Instance.Play(results.Tracks.First());

            UpdateUIDelegate = UpdateUI;

            UsersView.ItemsSource = users;
            PlaylistView.ItemsSource = pl._tracks;
            HistoryView.ItemsSource = history;
        }

        public static void NancyRequest(Track track){
            pl.AddByRef(track);
            UpdateUIDelegate(); // This shit right here is fucking retarded, WPF crashes if items are not refreshed, and nancy does not allow delegates....
        }

        public void UpdateUI() {
            Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                PlaylistView.Items.Refresh();
            }));
        }

        private void TrackEnded() {
            PTrack next = pl.NextTrack(users);
            history.Add(next);
            PlaylistView.Items.Refresh();
            HistoryView.Items.Refresh();
            SpotifyLoggedIn.Instance.Play(next.Track);
        }

        private void OnLoginSuccess(SpotifyLoggedIn spotifyLoggedIn)
        {
            Dispatcher.Invoke((Action)(() =>  {
                LoggedInStatus.Content = "Succesfully logged in";
                PlayButton.IsEnabled = true;
                StopButton.IsEnabled = true;
            }));

            //spotifyLoggedIn.Search("key");
        }

        void OnLogInError(LoginState loginState)
        {
                Dispatcher.Invoke((Action)(() => {
                    LoggedInStatus.Content = "Error: " + loginState.ToString();
                }));
        }

        private void OnRecieveData(int sample_rate, int channels, byte[] frames)
        {
            if (activeFormat == null)
                activeFormat = new NAudio.Wave.WaveFormat(sample_rate, 16, channels);

            if (sampleStream == null)
            {
                sampleStream = new NAudio.Wave.BufferedWaveProvider(activeFormat);
                sampleStream.DiscardOnBufferOverflow = true;
                sampleStream.BufferLength = 3000000;
                
            }

            if (waveOut == null)
            {
                waveOut = new NAudio.Wave.WaveOut();
                waveOut.Init(sampleStream);
                waveOut.Play();
            }
            session.BufferedBytes = sampleStream.BufferedBytes;
            session.BufferedDuration = sampleStream.BufferedDuration;
            sampleStream.AddSamples(frames, 0, frames.Length);
        }

        //WPF Content
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            //Track tracks = SpotifyLoggedIn.Instance.TrackFromLink("spotify:track:7eWYXAP87TFfF7fn2LEL1b");
            //Track t = pl
            //SpotifyLoggedIn.Instance.Play(tracks);
            PTrack next = pl.NextTrack(users);
            history.Add(next);
            PlaylistView.Items.Refresh();
            HistoryView.Items.Refresh();
            SpotifyLoggedIn.Instance.Play(next.Track);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e) {
            SpotifyLoggedIn.Instance.Stop(); //Hammertime
            //Don't
            waveOut.Stop(); //Believin'
            waveOut = null;
            activeFormat = null;
            sampleStream = null;
        }

        private void RemoveTrack_Click(object sender, RoutedEventArgs e) {
            pl.Remove((PTrack)PlaylistView.SelectedItem);
            PlaylistView.Items.Refresh();
        }

        private void AddTrack_Click(object sender, RoutedEventArgs e) {
            pl.AddByURI("spotify:track:7eWYXAP87TFfF7fn2LEL1b");
            PlaylistView.Items.Refresh();
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e) {
            pl.MoveUp((PTrack)PlaylistView.SelectedItem);
            PlaylistView.Items.Refresh();
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e) {
            pl.MoveDown((PTrack)PlaylistView.SelectedItem);
            PlaylistView.Items.Refresh();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e) {
            //"jensstaermose@hotmail.com", "34AKPAKCRE77K"
            session.Login("jensstaermose@hotmail.com", "34AKPAKCRE77K", false, appkey);
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e) {
            StopButton_Click(sender, e);
            TrackEnded();
        }
    }
}
