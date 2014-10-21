using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OpenPlaylistApp
{
    public class SearchTabView : TabbedPage
    {
        public SearchTabView()
        {
            Title = "Search";
            var searchTrack = new SearchView(WebAPILib.SearchType.TRACK) { Title = "Tracks" };
            this.Children.Add(searchTrack);
            var searchAlbum = new SearchView(WebAPILib.SearchType.ALBUM) { Title = "Albums" };
            this.Children.Add(searchAlbum);
            var searchArtist = new SearchView(WebAPILib.SearchType.ARTIST) { Title = "Artists" };
            this.Children.Add(searchArtist);
        }
    }
}
