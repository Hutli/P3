using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using WebAPILib;

namespace OpenPlaylistApp
{
    class NowPlayingView : ContentView
    {
        public NowPlayingView()
        {
            Label nowPlaying = new Label { Text = "Now Playing:" };

            var progressBar = new ProgressBar
            {
                Progress = .2,
            };

            StackLayout layout = new StackLayout { Children = { nowPlaying, progressBar } };
            Content = layout;
        }
    }
}
