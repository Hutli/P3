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

        List<PlaylistTrack> history = new List<PlaylistTrack>(); 
       
        public static Action UpdateUIDelegate;
        private IMainWindowViewModel _viewModel;

        public MainWindow(IMainWindowViewModel viewModel){
            InitializeComponent();

            _viewModel = viewModel;

            var hostConfig = new HostConfiguration();
            hostConfig.UrlReservations.CreateAutomatically = true;

            var host = new NancyHost(hostConfig, new Uri("http://localhost:5555"));
            host.Start();
            
            session.TrackEnded += viewModel.TrackEnded;
            
            DataContext = viewModel;
        }

        private void OnLoginSuccess(SpotifyLoggedIn spotifyLoggedIn)
        {
            Dispatcher.Invoke((Action)(() =>  {
                LoggedInStatus.Content = "Succesfully logged in";
                PlayButton.IsEnabled = true;
                StopButton.IsEnabled = true;
            }));
        }

        void OnLogInError(LoginState loginState)
        {
                Dispatcher.Invoke((Action)(() => {
                    LoggedInStatus.Content = "Error: " + loginState.ToString();
                }));
        }

        //WPF Content
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.PlayButtonClicked();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e) {
            _viewModel.StopButtonClicked();
        }

        private void RemoveTrack_Click(object sender, RoutedEventArgs e) {
            
        }

        private void AddTrack_Click(object sender, RoutedEventArgs e) {
            
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e) {
            
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e) {
            
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
