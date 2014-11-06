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
        public void PlaylistNextTrackHasHighestVotes()
        { //Tests that the next track is the one with highest votes
            UserService users = new UserService();
            
            PlaylistService pl = new PlaylistService(users);
            PlaylistTrack PlaylistTrack1 = new PlaylistTrack("spotify:track:5HWfldQwYjuvDXp1hWMlAH");
            PlaylistTrack PlaylistTrack2 = new PlaylistTrack("spotify:track:66ybn25Oh0CZ8dvrNrVDI1");


            User a = new User("1234");
            User b = new User("2345");
            User c = new User("3456");

            pl.Add(PlaylistTrack1);
            pl.Add(PlaylistTrack2);

            a.Vote = PlaylistTrack2;
            b.Vote = PlaylistTrack1;
            c.Vote = PlaylistTrack1;

            users.Add(a); //votes for track 2
            Assert.Equal(PlaylistTrack2, pl.NextTrack()); //t1 has 0 votes, t2 has 1 vote, tests that t2 is next

            users.Add(b); //votes for track 1
            users.Add(c); //votes for track 1
            Assert.Equal(PlaylistTrack1, pl.NextTrack()); //t1 has 2 votes, t2 has 1 vote, tests that t1 is next
        }

        [Fact]
        public void PlaylistAddByURIAddsTrack() {
            UserService u = new UserService();
            PlaylistService pl = new PlaylistService(u);
            Assert.False(pl.Tracks.Any(e => e.Name == "Obliteration of the Weak"));
            pl.AddByURI("spotify:track:19pTAbMZmWsgGkYZ4v2TM1");
            Assert.True(pl.Tracks.Any(e => e.Name == "Obliteration of the Weak"));
        }

        [Fact]
        public void PlaylistAddAddsTrack() {
            UserService u = new UserService();
            PlaylistService pl = new PlaylistService(u);
            PlaylistTrack pt = new PlaylistTrack("spotify:track:19pTAbMZmWsgGkYZ4v2TM1");
            Assert.False(pl.Tracks.Contains(pt));
            pl.Add(pt);
            Assert.Single(pl.Tracks, pt);
        }

        public void PlaylistRemoveByTitleRemovesTrack() {
            UserService u = new UserService();
            PlaylistService pl = new PlaylistService(u);
            Assert.False(pl.Tracks.Any(e => e.Name == "Obliteration of the Weak"));
            pl.AddByURI("spotify:track:19pTAbMZmWsgGkYZ4v2TM1");
            Assert.True(pl.Tracks.Any(e => e.Name == "Obliteration of the Weak"));
            pl.RemoveByTitle("Obliteration of the Weak");
            Assert.False(pl.Tracks.Any(e => e.Name == "Obliteration of the Weak"));
        }
        #endregion

        #region PlaylistTrack
        [Fact]
        public void PlaylistTrackConstructor()
        {
            //Track t = _data.spl.TrackFromLink("spotify:track:19pTAbMZmWsgGkYZ4v2TM1").Result;
            PlaylistTrack pt = new PlaylistTrack("spotify:track:19pTAbMZmWsgGkYZ4v2TM1");
            Assert.Equal("Obliteration of the Weak", pt.Name);
            Assert.Equal(232000, pt.Duration);
        }
        #endregion

        #region SpotifyLoggedIn
        [Fact]
        public void SpotifyLoggedInTrackFromLinkLoadsCorrectly() {
            Track t = _data.spl.TrackFromLink("spotify:track:19pTAbMZmWsgGkYZ4v2TM1").Result;
            
            Assert.Equal("Obliteration of the Weak", t.Name);
            Assert.Equal(232000, t.Duration);
        }

        // This has never worked
        [Fact]
        public async void SpotifyLoggedInPlaylistFromLinkLoadsCorrectly() {
            List<Track> tracks = await _data.spl.PlaylistFromLink("spotify:user:1110455666:playlist:4O7mYohOtO6xXmmiCd4lRS");
            Assert.NotEmpty(tracks);
            Assert.True(tracks.First().Name == "The Worst Is Yet To Come", "Name of first element does not match");
            Assert.True(tracks.Last().Name == "Dear Father", "Name of last element does not match");
            Assert.True(tracks.Count == 200, "Track count does not match");
        }
        #endregion

    }
}
