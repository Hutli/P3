using OpenPlaylistApp.Models;
using OpenPlaylistApp.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OpenPlaylistApp.Views
{
    class NowPlayingView : ContentView
    {
        private Label _nowPlayingLabel = new Label();
        private Label _trackTitleLabel = new Label();
        private Image _trackImage = new Image();
        NowPlayingViewModel _npvm;
        private RelativeLayout layout = new RelativeLayout();
        private ProgressBar progressBar = new ProgressBar();
        public NowPlayingView()
        {
            _nowPlayingLabel.Text = "Now Playing:";

            App.User.VenueChanged += GetNowPlaying;

            _trackImage.Opacity = 0.55f;
            _trackImage.Aspect = Aspect.AspectFill;

            _nowPlayingLabel.Font = Font.BoldSystemFontOfSize(NamedSize.Micro); // Maybe implement side scrolling text if clipped by parent
            _trackTitleLabel.Font = Font.SystemFontOfSize(NamedSize.Large);
            _trackTitleLabel.TextColor = Color.White;
           
            layout.MinimumHeightRequest = 80f;
            layout.IsClippedToBounds = true;
            layout.HeightRequest = 100f;

            layout.Children.Add(_trackImage, Constraint.Constant(0), Constraint.Constant(0), Constraint.RelativeToParent((parent) => parent.Width),Constraint.RelativeToParent((parent) => parent.Height));
            layout.Children.Add(_nowPlayingLabel, Constraint.Constant(0));
            layout.Children.Add(_trackTitleLabel, Constraint.Constant(0), Constraint.RelativeToParent((parent) => (parent.Height / 2)-(_trackTitleLabel.Height/2)));
            layout.Children.Add(progressBar, Constraint.Constant(0), Constraint.RelativeToParent((parent) => parent.Height - progressBar.Height), Constraint.RelativeToParent((parent) => parent.Width));
            Content = layout;
        }

        void GetNowPlaying(Venue venue)
        {
            _npvm = new NowPlayingViewModel(venue);
            _npvm.LoadComplete += OnLoadComplete;
        }

        void OnLoadComplete()
        {
            _trackTitleLabel.Text = _npvm.Result.Name + " - " + _npvm.Result.Album.Artists[0].Name;
            _trackImage.Source = _npvm.Result.Album.Images[0].URL;
            progressBar.Progress = Convert.ToDouble(_npvm.Result.CurrentDurationStep) / Convert.ToDouble(_npvm.Result.Duration);
            progressBar.ProgressTo(1, Convert.ToUInt32(_npvm.Result.Duration - _npvm.Result.CurrentDurationStep), Easing.Linear);
        }
    }
}
