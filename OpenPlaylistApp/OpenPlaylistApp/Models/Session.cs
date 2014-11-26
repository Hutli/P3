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

        private Session()
        {
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
                var str = await content.ReadAsStringAsync();
                return str;
            }
        }

        public async Task<string> GetPlaylist(Venue venue)
        {
            App.Home.IsBusy = true;
            UriBuilder uriBuilder = new UriBuilder("http", venue.IP, 5555, "playlist");
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
            using (HttpContent content = response.Content)
            {
                var str = await content.ReadAsStringAsync();
                App.Home.IsBusy = false;
                return str;
            }
        }

        public async Task<string> GetNowPlaying(Venue venue)
        {
            App.Home.IsBusy = true;
            UriBuilder uriBuilder = new UriBuilder("http", venue.IP, 5555, "nowplaying");
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
            using (HttpContent content = response.Content)
            {
                var str = await content.ReadAsStringAsync();
                App.Home.IsBusy = false;
                return str;
            }
        }

        public async Task<string> SetVolume(Venue venue, int volume, User user)
        {
            App.Home.IsBusy = true;
            UriBuilder uriBuilder = new UriBuilder("http", venue.IP, 5555, "volume/" + volume + "/" + user.Id);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now; //Else Windows Phone will cache and not make new request to the server
                using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
                using (HttpContent content = response.Content)
                {
                    var str = await content.ReadAsStringAsync();
                    App.Home.IsBusy = false;
                    return str;
                }
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

        public async Task<string> SendVote(Venue venue, Track track, User user)
        {
            App.Home.IsBusy = true;
            Random r = new Random();
            UriBuilder uriBuilder = new UriBuilder("http", venue.IP, 5555, "vote/" + track.URI + "/" + user.Id);
            using (HttpClient client = new HttpClient())
            {
                {
                    client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now;
                        //Else Windows Phone will cache and not make new request to the server

                    using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
                    using (HttpContent content = response.Content)
                    {
                        var str = await content.ReadAsStringAsync();
                        App.Home.IsBusy = false;
                        return str;
                    }
                }
            }
        }

        public async Task<string> Search(Venue venue, string searchStr)
        {
            App.Home.IsBusy = true;
            UriBuilder uriBuilder = new UriBuilder("http", venue.IP, 5555, "search/" + searchStr);
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
            using (HttpContent content = response.Content)
            {
                var str = await content.ReadAsStringAsync();
                App.Home.IsBusy = false;
                return str;
            }
        }
    }
}
