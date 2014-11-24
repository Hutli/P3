using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI;
using Xamarin.Forms;

namespace OpenPlaylistApp.Models
{
    public class Session
    {
        public static Session session;

        private Session() {
        }

        public static Session Instance()
        {
            return session ?? (session = new Session());
        }

        public async Task<string> GetVenues()
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync("http://op.zz.vc/"))
            using (HttpContent content = response.Content)
            {
                return await content.ReadAsStringAsync();
            }
        }

        public async Task<string> GetPlaylist(Venue venue)
        {
            UriBuilder uriBuilder = new UriBuilder("http", venue.IP, 5555, "playlist");
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
            using (HttpContent content = response.Content)
            {
                return await content.ReadAsStringAsync();
            }
        }

        public async void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is Track)) return;
            Track track = (Track)e.SelectedItem;
            Session session = Session.Instance();

            try
            {
                var json = await session.SendVote(App.User.Venue, track, App.User); //TODO vi bruger ikke variablen
            }
            catch (Exception ex)
            {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }

        public async Task<string> SendVote(Venue venue, Track track, User user){
            UriBuilder uriBuilder = new UriBuilder("http", venue.IP, 5555, track.URI + "/" + user.Id);
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
            using (HttpContent content = response.Content)
            {
                return await content.ReadAsStringAsync();
            }
        }

        public async Task<string> Search(Venue venue, string searchStr)
        {
            UriBuilder uriBuilder = new UriBuilder("http", venue.IP, 5555, "search/" + searchStr);
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
            using (HttpContent content = response.Content)
            {
                return await content.ReadAsStringAsync();
            }
        }
    }
}
