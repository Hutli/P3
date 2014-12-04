using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Nancy.Hosting.Self;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;
using SpotifyDotNet;
using Track = WebAPI.Track;

namespace OpenPlaylistServer.Services.Implementation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMainWindow
    {
        Spotify _session = Spotify.Instance;
        private static readonly byte[] _appkey = Properties.Resources.spotify_appkey;
       
        public static Action UpdateUIDelegate;
        private IMainWindowViewModel _viewModel;

        public MainWindow(IMainWindowViewModel viewModel){
            DataContext = viewModel;
            InitializeComponent();

            _viewModel = viewModel;

            var hostConfig = new HostConfiguration {UrlReservations = {CreateAutomatically = true}};

            var host = new NancyHost(hostConfig, new Uri("http://localhost:5555"));
            host.Start();
            
            _session.TrackEnded += viewModel.TrackEnded;
            
            
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
            //_viewModel.RemoveTrack_Click((Track)sender);
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

        private void AddFilter_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddRestriction(new Restriction(track => !track.Name.Contains("Monica"), new TimeSpan(4, 10, 0), new TimeSpan(7, 50, 0)));
            _viewModel.AddRestriction(new Restriction(track => track.Name != "Still Alive", new TimeSpan(4, 10, 0), new TimeSpan(7, 50, 0)));
        }

        private void RemoveFilter_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
