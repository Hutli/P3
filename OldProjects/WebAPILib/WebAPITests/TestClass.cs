using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using WebAPILib;

namespace WebAPITests
{
    //Test-playlist for WebAPI
    public class WebAPITests
    {
        #region WebAPILib.search
        [Fact]
        public void searchAddArtist() //Tests search.addArtist
        {
            search s = new search("dad", SearchType.ALL);
            Artist a = new Artist("1234", "testArtist", s);

            Assert.False(s.Artists.Contains(a));
            s.addArtist(a);
            Assert.True(s.Artists.Contains(a));
            s.addArtist(a);
            Assert.Single(s.Artists, a);
        }

        [Fact]
        public void searchAddAlbum() //Tests search.addArtist
        {
            search s = new search("dad", SearchType.ALL);
            Artist a = new Artist("1234", "testArtist", s);
            Image img1 = new Image(640, 640, "https://i.scdn.co/image/f8717f432506ab213c4de0c66d6ac24cd07ecf72");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/502bc1e1726e2594cd0045473e10d9166fa79dd8");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/d709f676d5c16db8941b1084b3ca348d35de04af");
            List<Image> imgs = new List<Image> { img1, img2, img3 };
            Album alb = new Album("1234", "testAlbum", "asdf", imgs, s);

            Assert.False(s.Albums.Contains(alb));
            s.addAlbum(alb);
            Assert.True(s.Albums.Contains(alb));
            s.addAlbum(alb);
            Assert.Single(s.Albums, alb);
        }

        [Fact]
        public void searchAddTrack() //Tests search.addTrack
        {
            search s = new search("dad", SearchType.ALL);
            List<Artist> artists = new List<Artist> { new Artist("1234", "testArtist", s) };
            Image img1 = new Image(640, 640, "https://i.scdn.co/image/f8717f432506ab213c4de0c66d6ac24cd07ecf72");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/502bc1e1726e2594cd0045473e10d9166fa79dd8");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/d709f676d5c16db8941b1084b3ca348d35de04af");
            List<Image> imgs = new List<Image> { img1, img2, img3 };
            Album alb = new Album("1234", "testAlbum", "asdf", imgs, s);
            Track t = new Track("1234", "testTrack", 10, 10, true, 10, alb, s, artists);

            Assert.False(s.Tracks.Contains(t));
            s.addTrack(t);
            Assert.True(s.Tracks.Contains(t));
            s.addTrack(t);
            Assert.Single(s.Tracks, t);
        }

        [Fact]
        public void searchNoDuplicatesInResults() //Tests for duplicates in search results
        {
            search s = new search("dad", SearchType.ALL);
            foreach (Artist a in s.Artists)
            {
                Assert.Single(s.Artists, a);
            }
            foreach (Album a in s.Albums)
            {
                Assert.Single(s.Albums, a);
            }
            foreach (Track a in s.Tracks)
            {
                Assert.Single(s.Tracks, a);
            }
        }

        [Fact]
        public void searchGetJObject() //Tests the getJObject method
        {
            JObject jo = search.getJobject("https://api.spotify.com/v1/albums/3j3cgkuyo015dghNYhHnZJ");
            Assert.True(jo.HasValues);
        }
        #endregion

        #region WebAPILib.Artist
        [Fact]
        public void ArtistAddAlbum() // Tests Artist.AddAlbum
        {
            search s = new search("dad", SearchType.ALL);
            Artist a = new Artist("1234", "testArtist", s);
            Image img1 = new Image(640, 640, "https://i.scdn.co/image/f8717f432506ab213c4de0c66d6ac24cd07ecf72");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/502bc1e1726e2594cd0045473e10d9166fa79dd8");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/d709f676d5c16db8941b1084b3ca348d35de04af");
            List<Image> imgs = new List<Image> { img1, img2, img3 };
            Album alb = new Album("1234", "testAlbum", "asdf", imgs, s);

            Assert.False(a.Albums.Contains(alb));
            a.addAlbum(alb);
            Assert.True(a.Albums.Contains(alb));
            a.addAlbum(alb);
            Assert.Single(a.Albums, alb);
        }
        #endregion

        #region WebAPILib.Album
        [Fact]
        public void AlbumAddArtist() // Tests Album.AddArtist
        {
            search s = new search("dad", SearchType.ALL);
            Artist a = new Artist("1234", "testArtist", s);
            Artist b = new Artist("2345", "testArtist2", s);
            List<Artist> artists = new List<Artist> { a, b };
            Image img1 = new Image(640, 640, "https://i.scdn.co/image/f8717f432506ab213c4de0c66d6ac24cd07ecf72");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/502bc1e1726e2594cd0045473e10d9166fa79dd8");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/d709f676d5c16db8941b1084b3ca348d35de04af");
            List<Image> imgs = new List<Image> { img1, img2, img3 };
            Album alb = new Album("3j3cgkuyo015dghNYhHnZJ", "testAlbum", "asdf", imgs, s);
            Album alb2 = new Album("3j3cgkuyo015dghNYhHnZJ", "testAlbum", "asdf", imgs, s);

            foreach (Artist art in artists)
            {
                Assert.False(alb.Artists.Contains(art));
            }
            alb2.addArtists(artists);
            foreach (Artist art in artists)
            {
                Assert.True(alb2.Artists.Contains(art));
            }
            foreach (Artist art in alb.Artists)
            {
                Assert.Single(alb.Artists, art);
            }
        }

        [Fact]
        public void AlbumAddTrack() // Tests Album.AddTrack
        {
            search s = new search("dad", SearchType.ALL);
            Artist a = new Artist("1234", "testArtist", s);
            Artist b = new Artist("2345", "testArtist2", s);
            List<Artist> artists = new List<Artist> { a, b };
            Image img1 = new Image(640, 640, "https://i.scdn.co/image/f8717f432506ab213c4de0c66d6ac24cd07ecf72");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/502bc1e1726e2594cd0045473e10d9166fa79dd8");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/d709f676d5c16db8941b1084b3ca348d35de04af");
            List<Image> imgs = new List<Image> { img1, img2, img3 };
            Album alb = new Album("3j3cgkuyo015dghNYhHnZJ", "testAlbum", "asdf", imgs, s);
            Album alb2 = new Album("3j3cgkuyo015dghNYhHnZJ", "testAlbum", "asdf", imgs, s);
            Track t = new Track("1234", "testTrack", 10, 10, true, 10, alb, s, artists);


            alb2.addTrack(t);
            Assert.True(alb2.Tracks.Contains(t));
            alb2.addTrack(t);
            Assert.Single(alb2.Tracks, t);
            foreach (Track track in alb.Tracks)
            {
                Assert.Single(alb.Tracks, track);
            }
        }
        #endregion
    }
}
