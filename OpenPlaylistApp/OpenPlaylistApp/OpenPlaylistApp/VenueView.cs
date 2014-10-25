using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    public class VenueView : ContentPage
    {

        Entry user;
        Entry venue;

        public VenueView()
        {
            BindingContext = new VenueViewModel();

            var layout = new StackLayout() { Spacing = 0 };

            user = new Entry {Placeholder = "User"};
            venue = new Entry { Placeholder = "IP of Venue" };
            var button = new Button
            {
                Text = "Submit",
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            button.Clicked += Button_Clicked;

            layout.Children.Add(user);
            layout.Children.Add(venue);
            layout.Children.Add(button);

            Content = layout;
        }

        public async void Button_Clicked(object sender,EventArgs args){
            App.user.Name = user.Text.ToString();
            App.venue.IP = venue.Text.ToString();
        }
    }
}
