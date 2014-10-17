using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OpenPlaylistServer;

namespace TestSuite
{
    public class TestPlaylist
    {

        [Fact]
        public void TestNextTrack()
        {
            List<User> users = new List<User>();
            Playlist pl = new Playlist();

            PTrack pTrack1 = new PTrack();
            PTrack pTrack2 = new PTrack();

            User a = new User();
            User b = new User();
            User c = new User();
            a.Vote = pTrack1;
            b.Vote = pTrack2;
            c.Vote = pTrack1;
            users.Add(a);
            users.Add(b);
            users.Add(c);

            pl._tracks.Add(pTrack1);
            pl._tracks.Add(pTrack2);

            Assert.Equal(pTrack1, pl.NextTrack(users));
        }

        [Fact]
        public void TestCurrentStanding(){
            List<User> users = new List<User>();
            Playlist pl = new Playlist();

            PTrack pTrack1 = new PTrack();

            User a = new User();

            users.Add(a);
            a.Vote = pTrack1;

            pl._tracks.Add(pTrack1);

            Assert.Equal(0, pTrack1.TScore);

            pl.CurrentStanding(users);

            Assert.Equal(1, pTrack1.TScore);
        }
    }
}
