using System;
using System.Collections.Generic;
using System.Linq;
using OpenPlaylistServer.Collections;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Implementation;
using SpotifyDotNet;
using WebAPI;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using Track = WebAPI.Track;

namespace TestSuite {
    #region Login
    public class TestsFixture : IDisposable {
        public SpotifyLoggedIn Spl;
        ManualResetEvent _man = new ManualResetEvent(false);

        //[Fact]
        public TestsFixture()
        {
            var result = Spotify.Instance.Login("jensstaermose@hotmail.com", "hejheider", false, Properties.Resources.spotify_appkey);
            Assert.True(result.Item2 == LoginState.OK, "Could not login to Spotify");
            Spl = result.Item1;
        }
        public void Dispose() {
        }
    }
    #endregion
    public class TestPlaylist : IUseFixture<TestsFixture> {
        private TestsFixture _data;

        public void SetFixture(TestsFixture data) {
            _data = data;
        }
        #region Server
        #region Services
        #region PlaylistService

        [Fact]
        public void PlaylistFindTrackFindsTrack()
        {
            UserService users = new UserService();
            HistoryService hist = new HistoryService();
            PlaylistService pl = new PlaylistService(users, hist);

            Image img1 = new Image(600, 600, "https://i.scdn.co/image/6885d1703f4f4fcbedd7beb231ecca8131de5683");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/c70d12c712e41ed4f532e4d190f3476380d0f708");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/cae856966342ec081a5dae800bb0efc8f8993612");
            IEnumerable<Image> images = new List<Image> { img1, img2, img3 };
            Artist art = new Artist("40UIlN4YEByXy4ewEZmqXu", "Aphyxion");
            List<Artist> artists = new List<Artist> { art };
            Album alb = new Album("0hNtREj1dl7bKoWEz0XXMr", "Obliteration of the Weak", "album", images, artists);

            Track track1 = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001", "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);
            Track track2 = new Track("50JtiuLYhzRO86ozFGxqYL", "Crossing the Boundaries", 220400, false, 2, "DKFD51642002", "https://p.scdn.co/mp3-preview/1dbb52c571db58003ecd0eb9dcf0d09c8bc4f0bb", alb);

            pl.Add(track1);
            pl.Add(track2);

            Assert.True(pl.FindTrack(track1.URI).Equals(track1));
            Assert.False(pl.FindTrack(track1.URI).Equals(track2));
        }

        [Fact]
        public void PlaylistCalcTScoreCalculatesScore()
        {
            UserService users = new UserService();
            HistoryService hist = new HistoryService();
            PlaylistService pl = new PlaylistService(users, hist);

            Image img1 = new Image(600, 600, "https://i.scdn.co/image/6885d1703f4f4fcbedd7beb231ecca8131de5683");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/c70d12c712e41ed4f532e4d190f3476380d0f708");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/cae856966342ec081a5dae800bb0efc8f8993612");
            IEnumerable<Image> images = new List<Image> { img1, img2, img3 };
            Artist art = new Artist("40UIlN4YEByXy4ewEZmqXu", "Aphyxion");
            List<Artist> artists = new List<Artist> { art };
            Album alb = new Album("0hNtREj1dl7bKoWEz0XXMr", "Obliteration of the Weak", "album", images, artists);

            Track track1 = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001", "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);
            Track track2 = new Track("50JtiuLYhzRO86ozFGxqYL", "Crossing the Boundaries", 220400, false, 2, "DKFD51642002", "https://p.scdn.co/mp3-preview/1dbb52c571db58003ecd0eb9dcf0d09c8bc4f0bb", alb);
            User a = new User("1234");
            User b = new User("2345");

            users.Add(a);
            users.Add(b);
            pl.Add(track1);
            pl.Add(track2);


            Assert.True(pl.CalcTScore(track1).Equals(0));
            a.Vote = track1;
            Assert.True(pl.CalcTScore(track1).Equals(1));
            b.Vote = track1;
            Assert.True(pl.CalcTScore(track1).Equals(2));
            a.Vote = track2;
            Assert.True(pl.CalcTScore(track1).Equals(1));
        }

        [Fact]
        public void PlaylistNextTrackHasHighestVotes()
        { 
            //Tests that the next track is the one with highest votes
            UserService users = new UserService();
            HistoryService hist = new HistoryService();
            PlaylistService pl = new PlaylistService(users, hist);

            Image img1 = new Image(600, 600, "https://i.scdn.co/image/6885d1703f4f4fcbedd7beb231ecca8131de5683");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/c70d12c712e41ed4f532e4d190f3476380d0f708");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/cae856966342ec081a5dae800bb0efc8f8993612");
            IEnumerable<Image> images = new List<Image> {img1, img2, img3};
            Artist art  = new Artist("40UIlN4YEByXy4ewEZmqXu", "Aphyxion");
            List<Artist> artists = new List<Artist> {art};
            Album alb = new Album("0hNtREj1dl7bKoWEz0XXMr","Obliteration of the Weak", "album", images, artists);

            Track track1 = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001", "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);
            Track track2 = new Track("50JtiuLYhzRO86ozFGxqYL", "Crossing the Boundaries", 220400, false, 2, "DKFD51642002", "https://p.scdn.co/mp3-preview/1dbb52c571db58003ecd0eb9dcf0d09c8bc4f0bb", alb);
            User a = new User("1234");
            User b = new User("2345");
            User c = new User("3456");

            pl.Add(track1);
            pl.Add(track2);

            a.Vote = track2;
            b.Vote = track1;
            c.Vote = track1;

            users.Add(a); //votes for track 2
            Assert.Equal(track2, pl.NextTrack()); //t1 has 0 votes, t2 has 1 vote, tests that t2 is next

            a.Vote = track2;
            users.Add(b); //votes for track 1
            users.Add(c); //votes for track 1
            
            Assert.Equal(track1, pl.NextTrack()); //t1 has 2 votes, t2 has 1 vote, tests that t1 is next
        }

        [Fact]
        public void PlaylistNextTrackFindsTrackIfEmpty()
        {
            UserService users = new UserService();
            HistoryService hist = new HistoryService();
            PlaylistService pl = new PlaylistService(users, hist);

            Image img1 = new Image(600, 600, "https://i.scdn.co/image/6885d1703f4f4fcbedd7beb231ecca8131de5683");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/c70d12c712e41ed4f532e4d190f3476380d0f708");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/cae856966342ec081a5dae800bb0efc8f8993612");
            IEnumerable<Image> images = new List<Image> { img1, img2, img3 };
            Artist art = new Artist("40UIlN4YEByXy4ewEZmqXu", "Aphyxion");
            List<Artist> artists = new List<Artist> { art };
            Album alb = new Album("0hNtREj1dl7bKoWEz0XXMr", "Obliteration of the Weak", "album", images, artists);
            Track track = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001", "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);
            
            users.Add(new User("1234") {Vote = track});
            pl.Add(track);

            var track1 = pl.NextTrack();
            hist.Add(track1);
            var track2 = pl.NextTrack();

            Assert.True(track.Equals(track1));
            Assert.NotNull(track2);
            Assert.False(track.Equals(track2));
        }

        [Fact]
        public void PlaylistAddAddsTrack() {
            UserService u = new UserService();
            HistoryService history = new HistoryService();
            PlaylistService pl = new PlaylistService(u, history);
            Track pt = new Track();
            Assert.False(pl.Tracks.Contains(pt));
            pl.Add(pt);
            Assert.Single(pl.Tracks, pt);
        }
        #endregion

        #region PlaybackService
        [Fact]
        public void PlaybackGetCurrentVolWorks()
        {
            UserService userService = new UserService();
            PlaybackService playback = new PlaybackService(userService);

            User u = new User("1234");
            userService.Add(u);
            Assert.True(playback.GetCurrentVolume().Equals(0.5F));
            u.Volume = 0.9F;
            Assert.True(playback.GetCurrentVolume().Equals(u.Volume));
        } 
        #endregion
        #endregion

        #region WebAPI
        #region Track
        [Fact]
        public void TrackEquals()
        {
            Image img1 = new Image(600, 600, "https://i.scdn.co/image/6885d1703f4f4fcbedd7beb231ecca8131de5683");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/c70d12c712e41ed4f532e4d190f3476380d0f708");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/cae856966342ec081a5dae800bb0efc8f8993612");
            IEnumerable<Image> images = new List<Image> { img1, img2, img3 };
            Artist art = new Artist("40UIlN4YEByXy4ewEZmqXu", "Aphyxion");
            List<Artist> artists = new List<Artist> { art };
            Album alb = new Album("0hNtREj1dl7bKoWEz0XXMr", "Obliteration of the Weak", "album", images, artists);
            Track track1 = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001", "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);
            Track track2 = new Track("50JtiuLYhzRO86ozFGxqYL", "Crossing the Boundaries", 220400, false, 2, "DKFD51642002", "https://p.scdn.co/mp3-preview/1dbb52c571db58003ecd0eb9dcf0d09c8bc4f0bb", alb);
            Track track1Cpy = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001", "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);
            Track emptyTrack1 = new Track();
            Track emptyTrack2 = new Track();
            Assert.False(track1.Equals(track2));
            Assert.True(track1.Equals(track1Cpy));
            Assert.True(emptyTrack1.Equals(emptyTrack2));
        }
        #endregion

        #region WebAPIMethods
        [Fact]
        public void WebAPIMethodsGetTrackGetsTrack()
        {
            Image img1 = new Image(600, 600, "https://i.scdn.co/image/6885d1703f4f4fcbedd7beb231ecca8131de5683");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/c70d12c712e41ed4f532e4d190f3476380d0f708");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/cae856966342ec081a5dae800bb0efc8f8993612");
            IEnumerable<Image> images = new List<Image> { img1, img2, img3 };
            Artist art = new Artist("40UIlN4YEByXy4ewEZmqXu", "Aphyxion");
            List<Artist> artists = new List<Artist> { art };
            Album alb = new Album("0hNtREj1dl7bKoWEz0XXMr", "Obliteration of the Weak", "album", images, artists);
            Track track1 = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001", "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);
            var foundTrack = WebAPIMethods.GetTrack(track1.URI).Result;
            Assert.True(foundTrack.Equals(track1));
        }

        [Fact]
        public async void WebAPIMethodsSearchGetsCorrectNoOfTracks()
        {
            for (int i = 1; i < 10; i++)
            {
                var tracks = await WebAPIMethods.Search("hej", 10);
                Assert.True(tracks.Count().Equals(10));
            }
        }
        #endregion
        #endregion

        #region SpotifyLoggedIn

        [Fact]
        public void LoggedInToSpotify()
        {
            Assert.NotNull(_data.Spl);
        }

        [Fact]
        public void SpotifyLoggedInTrackFromLinkLoadsCorrectly() {
            SpotifyDotNet.Track t = _data.Spl.TrackFromLink("spotify:track:19pTAbMZmWsgGkYZ4v2TM1").Result;
            
            Assert.Equal("Obliteration of the Weak", t.Name);
            Assert.Equal(232000, t.Duration);
        }
        #endregion

        #region Types
        #region ConcurrentDictify
        [Fact]
        public void ConcurrentDictifyAdd()
        {
            var condict = new ConcurrentDictify<string, int>();
            condict.Add("abc", 123);
            Assert.True(condict.ContainsKey("abc"));
            Assert.True(condict["abc"].Equals(123));
        }

        [Fact]
        public void ConcurrentDictifyRemove()
        {
            var condict = new ConcurrentDictify<string, int> {{"abc", 123}};
            Assert.True(condict.ContainsKey("abc"));
            condict.Remove("abc");
            Assert.False(condict.ContainsKey("abc"));
        }
        #endregion
        #endregion
        #endregion
    }
}
