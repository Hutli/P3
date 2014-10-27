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
    public partial class MainWindow : Window, IMainWindow
    {
        Spotify session = Spotify.Instance;
        private static byte[] appkey = OpenPlaylistServer.Properties.Resources.spotify_appkey;
        
        private static NAudio.Wave.WaveFormat activeFormat;
        private static NAudio.Wave.BufferedWaveProvider sampleStream;
        private static NAudio.Wave.WaveOut waveOut;

        //private static Playlist pl = new Playlist();
        List<PlaylistTrack> history = new List<PlaylistTrack>(); 
        static List<User> users = new List<User>();

        public static Action UpdateUIDelegate;
        private IMainWindowViewModel _viewModel;

        public MainWindow(IMainWindowViewModel viewModel){
            InitializeComponent();

            _viewModel = viewModel;

            var hostConfig = new HostConfiguration();
            hostConfig.UrlReservations.CreateAutomatically = true;

            var host = new NancyHost(hostConfig, new Uri("http://localhost:5555"));
            host.Start();
            //VoteModule.UserVoted += (userId, trackUri) =>
            //{
            //    Dispatcher.BeginInvoke((Action)(() => { UserVote(userId, trackUri); }));

            //};
            

            
            session.MusicDelivery += OnRecieveData;
            //session.TrackEnded += TrackEnded;
            session.TrackEnded += viewModel.TrackEnded;
            
            //session.SearchComplete += (results) => SpotifyLoggedIn.Instance.Play(results.Tracks.First());

            UpdateUIDelegate = UpdateUI;

            UsersView.ItemsSource = users;




            //var view = CollectionViewSource.GetDefaultView(pl._tracks);
            //view.SortDescriptions.Add(new SortDescription("TScore", ListSortDirection.Descending));
            //// do not show tracks without votes
            //var pred = new Predicate<Object>((obj) =>
            //{
            //    var playlistTrack = obj as playlistTrack;
            //    if (playlistTrack != null)
            //    {
            //        return playlistTrack.TotalScore < 1;
            //    }
            //    return false;
            //});

            //view.Filter += view_Filter;
            //view.Filter = pred;
            //PlaylistView.ItemsSource = view;
            
            //PlaylistView.ItemsSource = pl.Tracks;
            DataContext = viewModel;
            //PlaylistView.ItemsSource = viewModel.Tracks;

            HistoryView.ItemsSource = history;
        }

        //public MainWindow()
        //{
        //    InitializeComponent();
        //}

        //public async void UserVote(string userId, string trackUri)
        //{
        //    User user;
        //    Track track = await SpotifyLoggedIn.Instance.TrackFromLink(trackUri);

        //    // is playlistTrack already voted on?
        //    playlistTrack playlistTrack = pl.Tracks.FirstOrDefault(x => x.Track.Name == track.Name);
        //    if (playlistTrack == null)
        //    {
        //        // playlistTrack is not already voted on, so creating new instance and adding to list
        //        playlistTrack = new playlistTrack(track);
        //        pl.AddByRef(playlistTrack);
        //    }

        //    // Is user known?
        //    if (users.Any(x => x.Id == userId))
        //    {
        //        // User is known
        //        user = users.FirstOrDefault(x => x.Id == userId);
                
        //        // If user has already voted
        //        if (user.Vote != null)
        //        {
        //            // remove 1 vote on old track
        //            var oldVote = user.Vote;
        //            oldVote.TScore -= 1;
        //        }
        //    }
        //    else
        //    {
        //        // user is not known. Adding user to list of known users
        //        user = new User(userId);
        //        users.Add(user);
        //    }

        //    //  set user's vote to new track
        //    user.Vote = playlistTrack;
        //    user.Vote.TScore += 1;

        //    //UpdateUIDelegate();
        //}

        public void UpdateUI() {
            Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                //pl.CurrentStanding(users);
                //pl.Sort(pl._tracks);
                //PlaylistView.Items.Refresh();
                //UsersView.Items.Refresh();
            }));
        }

        //private void TrackEnded() {
        //    playlistTrack next = pl.NextTrack(users);
        //    history.Add(next);
        //    SpotifyLoggedIn.Instance.Play(next.Track);
        //    UpdateUI();
        //}

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
            _viewModel.PlayButonClicked();
            //Track tracks = SpotifyLoggedIn.Instance.TrackFromLink("spotify:track:7eWYXAP87TFfF7fn2LEL1b");
            //Track t = pl
            //SpotifyLoggedIn.Instance.Play(tracks);
            //playlistTrack next = pl.NextTrack(users);
            //_viewModel.NextTrack();
            ////history.Add(next);
            //PlaylistView.Items.Refresh();
            //HistoryView.Items.Refresh();
            //SpotifyLoggedIn.Instance.Play(next.Track);
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
            //pl.Remove((playlistTrack)PlaylistView.SelectedItem);
            //PlaylistView.Items.Refresh();
        }

        private void AddTrack_Click(object sender, RoutedEventArgs e) {
            //pl.AddByURI("spotify:track:6UEYM83XCHYiJ0C4Z10g6J");
            //PlaylistView.Items.Refresh();
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e) {
            //pl.MoveUp((playlistTrack)PlaylistView.SelectedItem);
            //PlaylistView.Items.Refresh();
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e) {
            //pl.MoveDown((playlistTrack)PlaylistView.SelectedItem);
            //PlaylistView.Items.Refresh();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e) {
            //"jensstaermose@hotmail.com", "34AKPAKCRE77K"
            Button test = (Button)sender;
            test.IsEnabled = false;
            var spotifyLoggedIn = await session.Login("jensstaermose@hotmail.com", "34AKPAKCRE77K", false, appkey);
            if (spotifyLoggedIn.Item2 == LoginState.OK)
            {
                OnLoginSuccess(spotifyLoggedIn.Item1);
            }
            else
            {
                OnLogInError(spotifyLoggedIn.Item2);
            }

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e) {
            StopButton_Click(sender, e);
            _viewModel.TrackEnded();
        }
    }
}
