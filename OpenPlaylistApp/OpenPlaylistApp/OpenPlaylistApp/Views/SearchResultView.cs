using System;
using System.Collections.ObjectModel;
using OpenPlaylistApp.Models;
using WebAPI;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace OpenPlaylistApp.Views
{

    public class SearchResultView : ContentView
    {
        public SearchResultView(string searchStr)
        {
            var layout = new StackLayout { Spacing = 0 };

            var activity = new ActivityIndicator
            {
                IsEnabled = true
            };

            GetResults(searchStr);

            activity.SetBinding(IsVisibleProperty, "IsBusy");
            activity.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");

            var list = new ListView {ItemsSource = HomePage.Search, ItemTemplate = new TrackTemplate()};

            list.ItemSelected += list_ItemSelected;

            layout.Children.Add(activity);
            layout.Children.Add(list);

            Content = layout;
        }

        public SearchResultView() { }

        async void GetResults(string searchStr)
        {
            Session session = Session.Instance();
            try
            {
                var json = await session.Search(App.User.Venue, searchStr);
                HomePage.Search = (ObservableCollection<Track>)JsonConvert.DeserializeObject(json, HomePage.Search.GetType());
                App.GetMainPage().DisplayAlert("Error", App.User.Venue.IP + " " + searchStr, "OK", "Cancel");
            }
            catch (Exception ex)
            {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }

        void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is Track)) return;
            Track track = (Track)e.SelectedItem;
            Session session = Session.Instance();
                
            try
            {
                var json = session.SendVote(App.User.Venue, track, App.User).Result; //TODO vi bruger ikke variablen
            } catch(Exception ex) {
                App.GetMainPage().DisplayAlert("Error",ex.Message, "OK", "Cancel");
            }
        }
    }
}
