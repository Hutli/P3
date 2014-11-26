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
        NowPlayingViewModel npvm;
        private RelativeLayout layout = new RelativeLayout();
        private ProgressBar progressBar = new ProgressBar();
        public NowPlayingView()
        {
            _nowPlayingLabel = new Label { Text = "Now Playing:" };

            App.User.VenueChanged += GetNowPlaying;

            _trackImage.Opacity = 0.55f;
            _trackImage.Aspect = Aspect.AspectFill;

            _nowPlayingLabel.Font = Font.BoldSystemFontOfSize(NamedSize.Micro); // Maybe implement side scrolling text if clipped by parent
            _trackTitleLabel.Font = Font.SystemFontOfSize(NamedSize.Large);
            _trackTitleLabel.TextColor = Color.White;
           
            layout.MinimumHeightRequest = 80f;
            layout.IsClippedToBounds = true;
            layout.HeightRequest = 100f;

            layout.Children.Add(_trackImage, Constraint.Constant(0), Constraint.Constant(0), Constraint.RelativeToParent((parent) => { return parent.Width; }),Constraint.RelativeToParent((parent) => { return parent.Height; }));
            layout.Children.Add(_nowPlayingLabel, Constraint.Constant(0));
            layout.Children.Add(_trackTitleLabel, Constraint.Constant(0), Constraint.RelativeToParent((parent) => { return (parent.Height / 2)-_trackTitleLabel.Height; }));
            layout.Children.Add(progressBar, Constraint.Constant(0), Constraint.RelativeToParent((parent) => { return parent.Height - progressBar.Height; }), Constraint.RelativeToParent((parent) => { return parent.Width; }));
            Content = layout;
        }

        void GetNowPlaying(Venue venue)
        {
            npvm = new NowPlayingViewModel(venue);
            npvm.LoadComplete += OnLoadComplete;
        }

        void OnLoadComplete()
        {
            _trackTitleLabel.Text = npvm.Result.Name + " - " + npvm.Result.Album.Artists[0].Name;
            _trackImage.Source = npvm.Result.Album.Images[0].URL;
            progressBar.Progress = Convert.ToDouble(npvm.Result.currentDurationStep) / Convert.ToDouble(npvm.Result.Duration);
            progressBar.ProgressTo(1, Convert.ToUInt32(npvm.Result.Duration - npvm.Result.currentDurationStep), Easing.Linear);
        }
    }
}
