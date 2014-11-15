using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SpotifyDotNet;
using Nancy.Hosting.Self;

namespace OpenPlaylistServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMainWindow
    {
        Spotify _session = Spotify.Instance;
        private static readonly byte[] _appkey = Properties.Resources.spotify_appkey;

        List<PlaylistTrack> _history = new List<PlaylistTrack>(); 
       
        public static Action UpdateUIDelegate;
        private IMainWindowViewModel _viewModel;

        public MainWindow(IMainWindowViewModel viewModel){
            InitializeComponent();

            _viewModel = viewModel;

            var hostConfig = new HostConfiguration {UrlReservations = {CreateAutomatically = true}};

            var host = new NancyHost(hostConfig, new Uri("http://localhost:5555"));
            host.Start();
            
            _session.TrackEnded += viewModel.TrackEnded;
            
            DataContext = viewModel;
        }

        private void OnLoginSuccess()
        {
            Dispatcher.Invoke(() =>  {
                                         LoggedInStatus.Content = "Succesfully logged in";
                                         PlayButton.IsEnabled = true;
                                         StopButton.IsEnabled = true;
            });
        }

        void OnLogInError(LoginState loginState)
        {
                Dispatcher.Invoke(() => {
                                            LoggedInStatus.Content = "Error: " + loginState;
                });
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
            var spotifyLoggedIn = await _session.Login("jensstaermose@hotmail.com", "34AKPAKCRE77K", false, _appkey);
            if (spotifyLoggedIn.Item2 == LoginState.OK)
            {
                OnLoginSuccess();
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
