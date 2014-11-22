using System;
using System.Collections.ObjectModel;
using OpenPlaylistApp.Models;
using WebAPI;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenPlaylistApp.Views
{

    public class SearchResultView : ContentView
    {
        public SearchResultView(string searchStr)
        {
            GetResults(searchStr);

            var list = new ListView {ItemsSource = HomePage.Search, ItemTemplate = new TrackTemplate()};

            list.ItemSelected += list_ItemSelected;

            Content = list;
        }

        async void GetResults(string searchStr)
        {
            Session session = Session.Instance();
            try
            {
                var json = await session.Search(App.User.Venue, searchStr);
                HomePage.Search = (ObservableCollection<Track>)JsonConvert.DeserializeObject(json, typeof(ObservableCollection<Track>));
                App.GetMainPage().DisplayAlert("Error", HomePage.Search[0].Name, "OK", "Cancel");
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
