using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI;
using Xamarin.Forms;

namespace OpenPlaylistApp.Models {
    public class Session {
        public static Session session;
        private static int _connectionFailures;

        private Session() {
            _connectionFailures = 0;
        }

        public static Session Instance() {
            return session ?? (session = new Session());
        }

        public static Uri MakeUri(string endpoint) {
            if(App.User == null || App.User.Venue == null)
                return null;
            var UriBuilder = new UriBuilder("http", App.User.Venue.IP, 5555, endpoint);
            return UriBuilder.Uri;
        }

        public static async Task<String> MakeRequest(Uri request,
                                                     string errorMessageTitle,
                                                     string errorMessage,
                                                     TimeSpan timeout,
                                                     bool loadIndicator) {
            if(_connectionFailures > 2) {
                App.Home.CheckOut();
                App.GetMainPage().DisplayAlert("Error", "Connection lost", "Ok", "Cancel");
                return null;
            }
            if(loadIndicator)
                App.Home.IsBusy = true;
            try {
                using(var client = new HttpClient()) {
                    client.Timeout = timeout;
                    //Else Windows Phone will cache and not make new request to the server
                    client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now;

                    using(var response = await client.GetAsync(request)) {
                        App.Home.IsBusy = false;
                        if(response.IsSuccessStatusCode) {
                            _connectionFailures = 0;
                            using(var content = response.Content) {
                                var str = await content.ReadAsStringAsync();
                                if(loadIndicator)
                                    App.Home.IsBusy = false;
                                return str;
                            }
                        }
                        if(loadIndicator)
                            App.Home.IsBusy = false;
                        return null;
                    }
                }
            } catch(Exception) {
                if(loadIndicator)
                    App.Home.IsBusy = false;
                if(errorMessageTitle.Equals("Venue not online"))
                    App.GetMainPage().DisplayAlert(errorMessageTitle, errorMessage, "Ok", "Cancel");
                _connectionFailures++;
                return null;
            }
        }

        public async Task<string> CheckIn(Venue venue, User user) {
            var UriBuilder = new UriBuilder("http", venue.IP, 5555, "checkin/" + user.Id);
            _connectionFailures = 0;
            return
                await
                MakeRequest(UriBuilder.Uri,
                            "Venue not online",
                            "The selected venue is not online. Try another one.",
                            new TimeSpan(0, 0, 5),
                            true);
        }

        public async Task<string> CheckOut(Venue venue, User user) {
            var UriBuilder = new UriBuilder("http", venue.IP, 5555, "checkout/" + user.Id);
            _connectionFailures = 0;
            return
                await
                MakeRequest(UriBuilder.Uri,
                            "Could not checkout",
                            "Checked in venue is offline",
                            new TimeSpan(0, 0, 5),
                            true);
        }

        public async Task<string> GetVenues() {
            var UriBuilder = new UriBuilder("http", "op.zz.vc");
            using(var client = new HttpClient()) {
                using(var response = await client.GetAsync("http://op.zz.vc/")) {
                    using(var content = response.Content) {
                        var str = await content.ReadAsStringAsync();
                        return str;
                    }
                }
            }
        }

        public async Task<string> GetPlaylist(Venue venue) {
            var UriBuilder = new UriBuilder("http", venue.IP, 5555, "playlist");

            return
                await
                MakeRequest(UriBuilder.Uri, "Playlist error", "Could not get playlist", new TimeSpan(0, 0, 10), false);
        }

        public async Task<string> GetNowPlaying(Venue venue) {
            var UriBuilder = new UriBuilder("http", venue.IP, 5555, "nowplaying");

            return
                await
                MakeRequest(UriBuilder.Uri,
                            "Nowplaying error",
                            "Could not get nowplaying",
                            new TimeSpan(0, 0, 10),
                            false);
        }

        public void ItemSelected(object sender, ItemTappedEventArgs e) {
            if(!(e.Item is Track))
                return;
            var track = (Track)e.Item;
            var session = Instance();
            try {
                session.SendVote(App.User.Venue, track, App.User);
                App.User.Vote = track;
            } catch(Exception ex) {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }

        public async void SendVote(Venue venue, Track track, User user) {
            var UriBuilder = new UriBuilder("http", venue.IP, 5555, "vote/" + track.Uri + "/" + user.Id);
            await
                MakeRequest(UriBuilder.Uri,
                            "Vote failed",
                            "Could not vote on selected track",
                            new TimeSpan(0, 0, 3),
                            true);
        }

        public async Task<string> Search(Venue venue, string searchStr, int offset = 0) {
            var UriBuilder = new UriBuilder("http", venue.IP, 5555, "search/" + searchStr + "/" + offset);
            return await MakeRequest(UriBuilder.Uri, "Search error", "Could not search", new TimeSpan(0, 0, 40), false);
        }
    }
}