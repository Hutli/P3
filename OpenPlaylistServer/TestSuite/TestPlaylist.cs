﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OpenPlaylistServer;
using SpotifyDotNet;
using System.Threading;

namespace TestSuite {
    public class TestsFixture : IDisposable
    {
        public Spotify sp = Spotify.Instance;
        public SpotifyLoggedIn spl;
        ManualResetEvent man = new ManualResetEvent(false);

        public TestsFixture()
        {
            var result = sp.Login("jensstaermose@hotmail.com", "34AKPAKCRE77K", false, TestSuite.Properties.Resources.spotify_appkey).Result;
            Assert.True(result.Item2 == LoginState.OK);
            spl = result.Item1;
            if (result.Item2 == LoginState.OK)
            {
                
            }
            else
            {
                
            }

            
        }

        public void Dispose()
        {
            //sp.Dispose();
        }
    }

    public class TestPlaylist : IUseFixture<TestsFixture> {

        private TestsFixture _data;

        public void SetFixture(TestsFixture data)
        {
            _data = data;
        }
        
        

        [Fact]
        public void NextTrackHasHighestVotes() { //Tests that the next track is the one with highest votes
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
        public void CurrentStandingGivesCorrectRes() {
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
        public void AddByURIAddsTrack() {
            Playlist pl = new Playlist();
            Assert.False(pl._tracks.Any(e => e.Name == "Obliteration of the Weak"));
            pl.AddByURI("spotify:track:19pTAbMZmWsgGkYZ4v2TM1");
            Assert.True(pl._tracks.Any(e => e.Name == "Obliteration of the Weak"));
        }

        //[Fact]
        //public void AddByRefAddsTrack() {   PROBLEM! Track har ingen constructor
        //    Playlist pl = new Playlist();
        //    Track t = new Track();
        //    PTrack pt = new PTrack(t);
        //    Assert.False(pl._tracks.Contains(pt));
        //    pl.AddByRef(t);
        //    Assert.Single(pl._tracks, pt);
        //} 

        
    }
}