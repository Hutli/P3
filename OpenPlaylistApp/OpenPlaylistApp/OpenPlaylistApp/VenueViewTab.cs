using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    public class VenueViewTab : TabbedPage
    {
        public VenueViewTab(){
            Title = "Venue";
            var venueView = new VenueView() { Title = "Tracks" };
            this.Children.Add(venueView);
        }
    }
}
