using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using WebAPILib;

namespace OpenPlaylistApp
{

    public class SearchResultView : ContentView
    {
        public SearchResultModel ViewModel
        {
            get
            {
                return BindingContext as SearchResultModel;
            }
        }

        public SearchResultView(string str, SearchType type)
        {
            BindingContext = new SearchResultModel(str, type);

            var layout = new StackLayout() { Spacing = 0 };

            var activity = new ActivityIndicator
            {
                IsEnabled = true
            };

            activity.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");
            activity.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");

            layout.Children.Add(activity);

            var list = new ListView();

            var cell = new TrackTemplate();

            list.ItemsSource = ViewModel.songs;

            list.ItemSelected += list_ItemSelected;

            layout.Children.Add(list);

            Content = layout;

            OnAppearing();

        }

        async void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Track)
            {
                Track track = (Track)e.SelectedItem;
                //PlaylistViewModel.vote = track;
                Session session = Session.Instance();
                
                try
                {
                    var json = await session.SendVote(App.venue, track, App.user);
                } catch(Exception ex) {
                    //throw new ConnectionToServerFaultedException("An error occured: '{0}'",ex);
                    
                    App.GetMainPage().DisplayAlert("Error",ex.Message, "OK", "Cancel");
                }
            }
        }

        protected void OnAppearing()
        {
            if (ViewModel == null || ViewModel.IsBusy || ViewModel.songs.Count > 0)
                return;

            ViewModel.LoadSongsCommand.Execute(null);

        }

    }
}
