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

        

        private async Task<String> MakeRequest(Uri request, string errorMessageTitle, string errorMessage, TimeSpan timeout)
        {
            App.Home.IsBusy = true;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = timeout;

                    using (HttpResponseMessage response = await client.GetAsync(request))
                    {
                        App.Home.IsBusy = false;
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var str = content.ReadAsStringAsync();
                                
                                return await str;


                                //return str;
                            }
                        }
                        else
                        {
                            return null;
                        }

                    }

                }
            }
            catch (Exception e)
            {
                App.Home.IsBusy = false;
                App.GetMainPage().DisplayAlert(errorMessageTitle, errorMessage, "Ok", "Cancel");
                return null;
                //throw;
            }
        }

        public async Task<string> CheckIn(Venue venue, User user)
        {
            UriBuilder uriBuilder = new UriBuilder("http", venue.IP, 5555, "checkin/" + user.Id);
            var str = await MakeRequest(uriBuilder.Uri, "Venue not online", "The selected venue is not online. Try another one.", new TimeSpan(0,0,3));

            return str;
        }

        public async Task<string> GetVenues()
        {
            //UriBuilder uriBuilder = new UriBuilder("http", "op.zz.vc");
            //return await MakeRequest(uriBuilder.Uri, "Venue list error",
            //    "Could not get list of venues. Contact your network administrator.");
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
            UriBuilder uriBuilder = new UriBuilder("http", venue.IP, 5555, "playlist");

            return await MakeRequest(uriBuilder.Uri, "Playlist error", "Could not get playlist", new TimeSpan(0,0,10));

            //using (HttpClient client = new HttpClient())
            //using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
            //using (HttpContent content = response.Content)
            //{
            //    var str = await content.ReadAsStringAsync();
            //    App.Home.IsBusy = false;
            //    return str;
            //}
        }

        public async Task<string> GetNowPlaying(Venue venue)
        {
            App.Home.IsBusy = true;
            UriBuilder uriBuilder = new UriBuilder("http", venue.IP, 5555, "nowplaying");
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now; //Else Windows Phone will cache and not make new request to the server
                using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
                using (HttpContent content = response.Content)
                {
                    return await content.ReadAsStringAsync();
                }
            }
        }

        public async Task<string> SetVolume(Venue venue, int volume, User user)
        {
            
            UriBuilder uriBuilder = new UriBuilder("http", venue.IP, 5555, "volume/" + volume + "/" + user.Id);

            return await MakeRequest(uriBuilder.Uri, "Volume error", "Could not set volume",new TimeSpan(0,0,3));


            //using (HttpClient client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now; //Else Windows Phone will cache and not make new request to the server
            //    using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
            //    using (HttpContent content = response.Content)
            //    {
            //        var str = await content.ReadAsStringAsync();
            //        App.Home.IsBusy = false;
            //        return str;
            //    }
            //}
        }

        public async void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is Track)) return;
            Track track = (Track)e.SelectedItem;
            Session session = Session.Instance();

            try
            {
                session.SendVote(App.User.Venue, track, App.User); //TODO vi bruger ikke variablen
                App.User.Vote = track;
            }
            catch (Exception ex)
            {
                App.GetMainPage().DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }

        public async void SendVote(Venue venue, Track track, User user)
        {
            UriBuilder uriBuilder = new UriBuilder("http", venue.IP, 5555, "vote/" + track.URI + "/" + user.Id);
            await MakeRequest(uriBuilder.Uri, "Vote failed", "Could not vote on selected track", new TimeSpan(0,0,3));
            //using (HttpClient client = new HttpClient())
            //{
            //    {
            //        client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now;
            //        //Else Windows Phone will cache and not make new request to the server

            //        using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
            //        using (HttpContent content = response.Content)
            //        {
            //            /*
            //            var str = await content.ReadAsStringAsync();
                        
            //            if (str != "Success")
            //            {
            //                throw new Exception("Vote failede");
            //            }*/
            //            App.Home.IsBusy = false;
            //        }
            //    }
            //}
        }

        public async Task<string> Search(Venue venue, string searchStr)
        {
            UriBuilder uriBuilder = new UriBuilder("http", venue.IP, 5555, "search/" + searchStr);

            return await MakeRequest(uriBuilder.Uri, "Search error", "Could not search", new TimeSpan(0, 0, 40));


            //using (HttpClient client = new HttpClient())
            //using (HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri))
            //using (HttpContent content = response.Content)
            //{
            //    var str = await content.ReadAsStringAsync();
            //    App.Home.browsePage.IsBusy = false;
            //    return str;
            //}
        }
    }
}
