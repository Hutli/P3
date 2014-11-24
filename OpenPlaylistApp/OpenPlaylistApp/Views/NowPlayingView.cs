using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    class NowPlayingView : ContentView
    {
        private Label _nowPlaying;
        public NowPlayingView()
        {
            _nowPlaying = new Label { Text = "Now Playing:" };

            var progressBar = new ProgressBar
            {
                Progress = .2,
            };

            StackLayout layout = new StackLayout { Children = { _nowPlaying, progressBar } };
            Content = layout;
        }
    }
}
