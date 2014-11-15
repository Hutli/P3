using System;
using Xamarin.Forms;
using WebAPILib;

namespace OpenPlaylistApp
{

    public class SearchResultView : ContentView
    {
        public SearchResultView()
        {
            var layout = new StackLayout { Spacing = 0 };

            var activity = new ActivityIndicator
            {
                IsEnabled = true
            };

            activity.SetBinding(IsVisibleProperty, "IsBusy");
            activity.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");

            var list = new ListView {ItemsSource = App.Search, ItemTemplate = new TrackTemplate()};

            list.ItemSelected += list_ItemSelected;

            layout.Children.Add(activity);
            layout.Children.Add(list);

            Content = layout;
        }

        async void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is Track)) return;
            Track track = (Track)e.SelectedItem;
            Session session = Session.Instance();
                
            try
            {
                var json = await session.SendVote(App.User.Venue, track, App.User); //TODO vi bruger ikke variablen
            } catch(Exception ex) {
                App.GetMainPage().DisplayAlert("Error",ex.Message, "OK", "Cancel");
            }
        }
    }
}
