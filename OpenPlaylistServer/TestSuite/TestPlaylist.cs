#define LoginToSpotify
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using OpenPlaylistServer.Collections;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Implementation;
using SpotifyDotNet;
using WebAPI;
using Xunit;
using Track = WebAPI.Track;

namespace TestSuite {
#region Login
    public class TestsFixture : IDisposable
    {
        public SpotifyLoggedIn Spl;
        public TestsFixture()
        {
            #if (!LoginToSpotify)
            return;
            #endif
            var result = Spotify.Instance.Login("jensstaermose@hotmail.com", "hejheider", false, Properties.Resources.spotify_appkey);
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
            var users = new UserService();
            var hist = new HistoryService();
            var pl = new PlaylistService(users, hist);

            var img1 = new Image(600, 600, "https://i.scdn.co/image/6885d1703f4f4fcbedd7beb231ecca8131de5683");
            var img2 = new Image(300, 300, "https://i.scdn.co/image/c70d12c712e41ed4f532e4d190f3476380d0f708");
            var img3 = new Image(64, 64, "https://i.scdn.co/image/cae856966342ec081a5dae800bb0efc8f8993612");
            IEnumerable<Image> images = new List<Image> { img1, img2, img3 };
            var art = new Artist("40UIlN4YEByXy4ewEZmqXu", "Aphyxion");
            var artists = new List<Artist> { art };
            var alb = new Album("0hNtREj1dl7bKoWEz0XXMr", "Obliteration of the Weak", "album", images, artists);

            var track1 = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001", "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);
            var track2 = new Track("50JtiuLYhzRO86ozFGxqYL", "Crossing the Boundaries", 220400, false, 2, "DKFD51642002", "https://p.scdn.co/mp3-preview/1dbb52c571db58003ecd0eb9dcf0d09c8bc4f0bb", alb);

            pl.Add(track1);
            pl.Add(track2);

            Assert.True(pl.FindTrack(track1.URI).Equals(track1));
            Assert.False(pl.FindTrack(track1.URI).Equals(track2));
        }

        [Fact]
        public void PlaylistCalcTScoreCalculatesScore()
        {
            var users = new UserService();
            var hist = new HistoryService();
            var pl = new PlaylistService(users, hist);

            var img1 = new Image(600, 600, "https://i.scdn.co/image/6885d1703f4f4fcbedd7beb231ecca8131de5683");
            var img2 = new Image(300, 300, "https://i.scdn.co/image/c70d12c712e41ed4f532e4d190f3476380d0f708");
            var img3 = new Image(64, 64, "https://i.scdn.co/image/cae856966342ec081a5dae800bb0efc8f8993612");
            IEnumerable<Image> images = new List<Image> { img1, img2, img3 };
            var art = new Artist("40UIlN4YEByXy4ewEZmqXu", "Aphyxion");
            var artists = new List<Artist> { art };
            var alb = new Album("0hNtREj1dl7bKoWEz0XXMr", "Obliteration of the Weak", "album", images, artists);

            var track1 = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001", "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);
            var track2 = new Track("50JtiuLYhzRO86ozFGxqYL", "Crossing the Boundaries", 220400, false, 2, "DKFD51642002", "https://p.scdn.co/mp3-preview/1dbb52c571db58003ecd0eb9dcf0d09c8bc4f0bb", alb);
            var a = new User("1234");
            var b = new User("2345");

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
            var users = new UserService();
            var hist = new HistoryService();
            var pl = new PlaylistService(users, hist);

            var img1 = new Image(600, 600, "https://i.scdn.co/image/6885d1703f4f4fcbedd7beb231ecca8131de5683");
            var img2 = new Image(300, 300, "https://i.scdn.co/image/c70d12c712e41ed4f532e4d190f3476380d0f708");
            var img3 = new Image(64, 64, "https://i.scdn.co/image/cae856966342ec081a5dae800bb0efc8f8993612");
            IEnumerable<Image> images = new List<Image> {img1, img2, img3};
            var art  = new Artist("40UIlN4YEByXy4ewEZmqXu", "Aphyxion");
            var artists = new List<Artist> {art};
            var alb = new Album("0hNtREj1dl7bKoWEz0XXMr","Obliteration of the Weak", "album", images, artists);

            var track1 = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001", "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);
            var track2 = new Track("50JtiuLYhzRO86ozFGxqYL", "Crossing the Boundaries", 220400, false, 2, "DKFD51642002", "https://p.scdn.co/mp3-preview/1dbb52c571db58003ecd0eb9dcf0d09c8bc4f0bb", alb);
            var a = new User("1234");
            var b = new User("2345");
            var c = new User("3456");

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
            var users = new UserService();
            var hist = new HistoryService();
            var pl = new PlaylistService(users, hist);

            var img1 = new Image(600, 600, "https://i.scdn.co/image/6885d1703f4f4fcbedd7beb231ecca8131de5683");
            var img2 = new Image(300, 300, "https://i.scdn.co/image/c70d12c712e41ed4f532e4d190f3476380d0f708");
            var img3 = new Image(64, 64, "https://i.scdn.co/image/cae856966342ec081a5dae800bb0efc8f8993612");
            IEnumerable<Image> images = new List<Image> { img1, img2, img3 };
            var art = new Artist("40UIlN4YEByXy4ewEZmqXu", "Aphyxion");
            var artists = new List<Artist> { art };
            var alb = new Album("0hNtREj1dl7bKoWEz0XXMr", "Obliteration of the Weak", "album", images, artists);
            var track = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001", "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);
            
            users.Add(new User("1234") {Vote = track});
            pl.Add(track);

            var track1 = pl.NextTrack();
            hist.Add(track1);
            Assert.Equal(track, track1);

            var track2 = pl.NextTrack();
            hist.Add(track2);
            Assert.NotNull(track2);
            Assert.NotEqual(track, track2);

            var track3 = pl.NextTrack();
            Assert.NotNull(track3);
            Assert.NotEqual(track3, track);
            Assert.NotEqual(track3, track2);
        }

        [Fact]
        public void PlaylistAddAddsTrack() {
            var u = new UserService();
            var history = new HistoryService();
            var pl = new PlaylistService(u, history);

            var img1 = new Image(600, 600, "https://i.scdn.co/image/6885d1703f4f4fcbedd7beb231ecca8131de5683");
            var img2 = new Image(300, 300, "https://i.scdn.co/image/c70d12c712e41ed4f532e4d190f3476380d0f708");
            var img3 = new Image(64, 64, "https://i.scdn.co/image/cae856966342ec081a5dae800bb0efc8f8993612");
            IEnumerable<Image> images = new List<Image> { img1, img2, img3 };
            var art = new Artist("40UIlN4YEByXy4ewEZmqXu", "Aphyxion");
            var artists = new List<Artist> { art };
            var alb = new Album("0hNtREj1dl7bKoWEz0XXMr", "Obliteration of the Weak", "album", images, artists);
            var track = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001", "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);
            
            Assert.False(pl.Tracks.Contains(track));
            pl.Add(track);
            Assert.Single(pl.Tracks, track);
        }
        #endregion

        #region PlaybackService
        [Fact]
        public void PlaybackGetCurrentVolWorks()
        {
            var userService = new UserService();
            var playback = new PlaybackService(userService);

            var u = new User("1234");
            userService.Add(u);
            Assert.True(playback.GetCurrentVolume().Equals(0.5F));
            u.Volume = 0.9F;
            Assert.True(playback.GetCurrentVolume().Equals(u.Volume));
            u.Volume = 2F;
            Assert.True(playback.GetCurrentVolume().Equals(0.9F));
            u.Volume = -0.5F;
            Assert.True(playback.GetCurrentVolume().Equals(0.9F));
        } 
        #endregion

        #region VoteService
        [Fact]
        public void VoteServiceVoteWorks()
        {
            var users = new UserService();
            var hist = new HistoryService();
            var pl = new PlaylistService(users, hist);
            var vs = new VoteService(pl, users);

            var u = new User("1234");
            users.Add(u);

            var img1 = new Image(600, 600, "https://i.scdn.co/image/6885d1703f4f4fcbedd7beb231ecca8131de5683");
            var img2 = new Image(300, 300, "https://i.scdn.co/image/c70d12c712e41ed4f532e4d190f3476380d0f708");
            var img3 = new Image(64, 64, "https://i.scdn.co/image/cae856966342ec081a5dae800bb0efc8f8993612");
            IEnumerable<Image> images = new List<Image> { img1, img2, img3 };
            var art = new Artist("40UIlN4YEByXy4ewEZmqXu", "Aphyxion");
            var artists = new List<Artist> { art };
            var alb = new Album("0hNtREj1dl7bKoWEz0XXMr", "Obliteration of the Weak", "album", images, artists);

            var track = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001", "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);
            pl.Add(track);

            Assert.NotNull(vs);
            Assert.NotNull(u);
            Assert.NotNull(track);

            Assert.True(vs.Vote(u.Id, track.URI));
            Assert.True(u.Vote.Track.Equals(track));

            //Assert.False(vs.Vote(u.Id, null));
            Assert.True(u.Vote.Track.Equals(track));


        }
        #endregion

        #region RestrictionService
        [Fact]
        public void RestrictionServiceRestrictTracksBlackTitlesWorks()
        {
            var resServ = new RestrictionService();
            var resUnit = new RestrictionUnit(TrackField.Titles, "hej");
            var res = new Restriction(new DateTime(), new DateTime(1, 1, 1, 23, 59, 59), RestrictionType.BlackList, resUnit);

            var searchRes = WebAPIMethods.Search("hej", 20).Result;

            Assert.True(searchRes.Any(t => t.Name.ToLower().Contains("hej") && !t.IsFiltered), "Search failed");

            resServ.AddRestriction(res);
            resServ.RestrictTracks(searchRes);

            Assert.True(searchRes.Where(t => t.Name.ToLower().Contains("hej")).All(t => t.IsFiltered), "Restriction failed");
        }

        [Fact]
        public void RestrictionServiceRestrictTrackWhiteTitlesWorks() {
            var resServ = new RestrictionService();
            var resUnit = new RestrictionUnit(TrackField.Titles, "hej");
            var res = new Restriction(new DateTime(), new DateTime(1, 1, 1, 23, 59, 59), RestrictionType.WhiteList, resUnit);

            var searchRes = WebAPIMethods.Search("hej", 20).Result;

            Assert.True(searchRes.Any(t => !t.Name.ToLower().Contains("hej")), "Search failed");

            resServ.AddRestriction(res);
            resServ.RestrictTracks(searchRes);

            Assert.True(searchRes.All(t => (t.Name.ToLower().Contains("hej") && !t.IsFiltered) || t.IsFiltered), "Restriction failed");
        }

        [Fact]
        public void RestrictionServiceRestrictTrackBlackArtistsWorks() {
            var resServ = new RestrictionService();
            var resUnit = new RestrictionUnit(TrackField.Artists, "hej");
            var res = new Restriction(new DateTime(), new DateTime(1, 1, 1, 23, 59, 59), RestrictionType.BlackList, resUnit);

            var searchRes = WebAPIMethods.Search("hej", 20).Result;

            Assert.True(searchRes.Any(t => t.Album.Artists.Any(a => a.Name.ToLower().Contains("hej"))), "Search failed");

            resServ.AddRestriction(res);
            resServ.RestrictTracks(searchRes);

            Assert.True(searchRes.All(t => 
                (t.Album.Artists.Any(a => a.Name.ToLower().Contains("hej")) && t.IsFiltered) 
                || (!t.Album.Artists.Any(a => a.Name.ToLower().Contains("hej")) && !t.IsFiltered)), "Restriction failed");
        }

        [Fact]
        public void RestrictionServiceRestrictTrackWhiteArtistsWorks() {
            var resServ = new RestrictionService();
            var resUnit = new RestrictionUnit(TrackField.Artists, "hej");
            var res = new Restriction(new DateTime(), new DateTime(1, 1, 1, 23, 59, 59), RestrictionType.WhiteList, resUnit);

            var searchRes = WebAPIMethods.Search("hej", 20).Result;

            Assert.True(searchRes.Any(t => t.Album.Artists.Any(a => a.Name.ToLower().Contains("hej"))), "Search failed");

            resServ.AddRestriction(res);
            resServ.RestrictTracks(searchRes);

            Assert.True(searchRes.All(t =>
                (t.Album.Artists.Any(a => a.Name.ToLower().Contains("hej")) && !t.IsFiltered)
                || (!t.Album.Artists.Any(a => a.Name.ToLower().Contains("hej")) && t.IsFiltered)), "Restriction failed");
        }
        #endregion
    #endregion

    #region WebAPI
        #region Track
        [Fact]
        public void TrackEquals()
        {
            var img1 = new Image(600, 600, "https://i.scdn.co/image/6885d1703f4f4fcbedd7beb231ecca8131de5683");
            var img2 = new Image(300, 300, "https://i.scdn.co/image/c70d12c712e41ed4f532e4d190f3476380d0f708");
            var img3 = new Image(64, 64, "https://i.scdn.co/image/cae856966342ec081a5dae800bb0efc8f8993612");
            IEnumerable<Image> images = new List<Image> { img1, img2, img3 };
            var art = new Artist("40UIlN4YEByXy4ewEZmqXu", "Aphyxion");
            var artists = new List<Artist> { art };
            var alb = new Album("0hNtREj1dl7bKoWEz0XXMr", "Obliteration of the Weak", "album", images, artists);
            var track1 = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001", "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);
            var track2 = new Track("50JtiuLYhzRO86ozFGxqYL", "Crossing the Boundaries", 220400, false, 2, "DKFD51642002", "https://p.scdn.co/mp3-preview/1dbb52c571db58003ecd0eb9dcf0d09c8bc4f0bb", alb);
            var track1Cpy = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001", "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);

            Assert.False(track1.Equals(track2));
            Assert.NotSame(track1, track1Cpy);
            Assert.Equal(track1, track1Cpy);
        }
        #endregion

        #region WebAPIMethods
        [Fact]
        public void WebAPIMethodsGetTrackGetsTrack()
        {
            //Information needed for track constructor
            var img1 = new Image(600, 600, "https://i.scdn.co/image/6885d1703f4f4fcbedd7beb231ecca8131de5683");
            var img2 = new Image(300, 300, "https://i.scdn.co/image/c70d12c712e41ed4f532e4d190f3476380d0f708");
            var img3 = new Image(64, 64, "https://i.scdn.co/image/cae856966342ec081a5dae800bb0efc8f8993612");
            IEnumerable<Image> images = new List<Image> { img1, img2, img3 };
            var art = new Artist("40UIlN4YEByXy4ewEZmqXu", "Aphyxion");
            var artists = new List<Artist> { art };
            var alb = new Album("0hNtREj1dl7bKoWEz0XXMr", "Obliteration of the Weak", "album", images, artists);

            //Construct track manually
            var track = new Track("19pTAbMZmWsgGkYZ4v2TM1", "Obliteration of the Weak", 232120, false, 1, "DKFD51642001"
                                    , "https://p.scdn.co/mp3-preview/1d3ee1111d679b5e5b50c53aa3bfcceb4c83da8a", alb);

            //Find track using WebAPI
            var foundTrack = WebAPIMethods.GetTrack(track.URI);

            Assert.Equal(track, foundTrack);
        }

        [Fact]
        public async void WebAPIMethodsSearchGetsCorrectNoOfTracks()
        {
            for (var i = 1; i < 10; i++)
            {
                var tracks = await WebAPIMethods.Search("hej", 10);
                Assert.True(tracks.Count().Equals(10));
            }
        }
        #endregion
    #endregion

    #region SpotifyLoggedIn
        [Fact]
        public void SpotifyLoggedInIsLoggedInToSpotify()
        {
            Assert.NotNull(_data.Spl);
        }

        [Fact]
        public void SpotifyLoggedInTrackFromLinkLoadsCorrectly() {
            var t = _data.Spl.TrackFromLink("spotify:track:19pTAbMZmWsgGkYZ4v2TM1").Result;
            
            Assert.Equal("Obliteration of the Weak", t.Name);
            Assert.Equal(232000, t.Duration);
        }

        [Fact]
        public void SpotifyLoggedInPlayWorks() {
            var t = _data.Spl.TrackFromLink("spotify:track:19pTAbMZmWsgGkYZ4v2TM1").Result;
            Assert.DoesNotThrow(() => _data.Spl.Play(t));
            Assert.Throws<NullReferenceException>(() => _data.Spl.Play(null));
        }

        [Fact]
        public void SpotifyLoggedInStopWorks()
        {
            var t = _data.Spl.TrackFromLink("spotify:track:19pTAbMZmWsgGkYZ4v2TM1").Result;
            _data.Spl.Play(t);
            Assert.DoesNotThrow(()=> _data.Spl.Stop());
        }

        [Fact]
        public void SpotifyLoggedInPlayAfterStopWorks() {
            var t = _data.Spl.TrackFromLink("spotify:track:19pTAbMZmWsgGkYZ4v2TM1").Result;
            Assert.DoesNotThrow(() => _data.Spl.Play(t));
            Assert.DoesNotThrow(() => _data.Spl.Stop());
            Assert.DoesNotThrow(() => _data.Spl.Play(t));
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
