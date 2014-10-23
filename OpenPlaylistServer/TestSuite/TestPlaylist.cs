using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OpenPlaylistServer;
using SpotifyDotNet;
using System.Threading;

namespace TestSuite {
    #region Login
    public class TestsFixture : IDisposable {
        public SpotifyLoggedIn spl;
        ManualResetEvent man = new ManualResetEvent(false);

        public TestsFixture() {
            Tuple<SpotifyLoggedIn, LoginState> result = Spotify.Instance.Login("jensstaermose@hotmail.com", "34AKPAKCRE77K", false, TestSuite.Properties.Resources.spotify_appkey).Result;
            Assert.True(result.Item2 == LoginState.OK, "Could not login to Spotify");
            spl = result.Item1;
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

        #region Playlist
        [Fact]
        public void PlaylistNextTrackHasHighestVotes() { //Tests that the next track is the one with highest votes
            List<User> users = new List<User>();
            Playlist pl = new Playlist();
            PTrack pTrack1 = new PTrack();
            PTrack pTrack2 = new PTrack();
            User a = new User("1234");
            User b = new User("2345");
            User c = new User("3456");

            pl._tracks.Add(pTrack1);
            pl._tracks.Add(pTrack2);

            a.Vote = pTrack2;
            b.Vote = pTrack1;
            c.Vote = pTrack1;

            users.Add(a); //votes for track 2
            Assert.Equal(pTrack2, pl.NextTrack(users)); //t1 has 0 votes, t2 has 1 vote, tests that t2 is next

            users.Add(b); //votes for track 1
            users.Add(c); //votes for track 1
            Assert.Equal(pTrack1, pl.NextTrack(users)); //t1 has 2 votes, t2 has 1 vote, tests that t1 is next
        }

        [Fact]
        public void PlaylistCurrentStandingGivesCorrectRes() {
            List<User> users = new List<User>();
            Playlist pl = new Playlist();
            PTrack pTrack1 = new PTrack();
            User a = new User("1234");

            users.Add(a);
            a.Vote = pTrack1;
            pl._tracks.Add(pTrack1);

            Assert.Equal(0, pTrack1.TScore);

            pl.CurrentStanding(users);

            Assert.Equal(1, pTrack1.TScore);
        }

        [Fact]
        public void PlaylistAddByURIAddsTrack() {
            Playlist pl = new Playlist();
            Assert.False(pl._tracks.Any(e => e.Name == "Obliteration of the Weak"));
            pl.AddByURI("spotify:track:19pTAbMZmWsgGkYZ4v2TM1");
            Assert.True(pl._tracks.Any(e => e.Name == "Obliteration of the Weak"));
        }

        [Fact]
        public async void PlaylistAddByRefAddsTrack() {
            Playlist pl = new Playlist();
            Track t = await _data.spl.TrackFromLink("spotify:track:19pTAbMZmWsgGkYZ4v2TM1");
            PTrack pt = new PTrack(t);
            Assert.False(pl._tracks.Contains(pt));
            pl.AddByRef(t);
            Assert.Single(pl._tracks, pt);
        }

        public void PlaylistRemoveByTitleRemovesTrack() {
            Playlist pl = new Playlist();
            Assert.False(pl._tracks.Any(e => e.Name == "Obliteration of the Weak"));
            pl.AddByURI("spotify:track:19pTAbMZmWsgGkYZ4v2TM1");
            Assert.True(pl._tracks.Any(e => e.Name == "Obliteration of the Weak"));
            pl.RemoveByTitle("Obliteration of the Weak");
            Assert.False(pl._tracks.Any(e => e.Name == "Obliteration of the Weak"));
        }
        #endregion

        #region PTrack
        [Fact]
        public void PTrackConstructor() {
            Track t = _data.spl.TrackFromLink("spotify:track:19pTAbMZmWsgGkYZ4v2TM1").Result;
            PTrack pt = new PTrack(t);
            Assert.True(pt.Name == "Obliteration of the Weak", "Name does not match");
            Assert.True(pt.Track == t, "Track does not match");
            Assert.True(pt.Duration == 232000, "Duration does not match");
        }
        #endregion

        #region SpotifyLoggedIn
        [Fact]
        public void SpotifyLoggedInTrackFromLinkLoadsCorrectly() {
            Track t = _data.spl.TrackFromLink("spotify:track:19pTAbMZmWsgGkYZ4v2TM1").Result;
            Assert.True(t.Name == "Obliteration of the Weak", "Name does not match");
            Assert.True(t.Duration == 232000, "Duration does not match");
        }

        [Fact]
        public void SpotifyLoggedInPlaylistFromLinkLoadsCorrectly() {
            List<Track> tracks = _data.spl.PlaylistFromLink("spotify:user:1166648382:playlist:7d1QMj15EBsNiG23ilPSOv").Result;
            Assert.NotEmpty(tracks);
            Assert.True(tracks.First().Name == "The Worst Is Yet To Come", "Name of first element does not match");
            Assert.True(tracks.Last().Name == "Dear Father", "Name of last element does not match");
            Assert.True(tracks.Count == 200, "Track count does not match");
        }
        #endregion

    }
}
