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
        public SearchResultView()
        {
            var layout = new StackLayout() { Spacing = 0 };

            var activity = new ActivityIndicator
            {
                IsEnabled = true
            };

            activity.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");
            activity.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");

            var list = new ListView();

            list.ItemsSource = App.search;
            list.ItemTemplate = new TrackTemplate();

            list.ItemSelected += list_ItemSelected;

            layout.Children.Add(activity);
            layout.Children.Add(list);

            Content = layout;
        }

        async void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Track)
            {
                Track track = (Track)e.SelectedItem;
                Session session = Session.Instance();
                
                try
                {
                    var json = await session.SendVote(App.user.Venue, track, App.user);
                } catch(Exception ex) {
                    App.GetMainPage().DisplayAlert("Error",ex.Message, "OK", "Cancel");
                }
            }
        }
    }
}
