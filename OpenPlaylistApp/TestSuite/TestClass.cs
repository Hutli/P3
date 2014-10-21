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
        #region WebAPILib.Search
        [Fact]
        public void SearchAllConstructor() {
            Search sAll = new Search("dad", SearchType.ALL);

            Assert.NotEmpty(sAll.Artists);
            foreach (Artist art in sAll.Artists) { Assert.Single(sAll.Artists, art); }
            Assert.NotEmpty(sAll.Albums);
            foreach (Album alb in sAll.Albums) { Assert.Single(sAll.Albums, alb); }
            Assert.NotEmpty(sAll.Tracks);
            foreach (Track t in sAll.Tracks) { Assert.Single(sAll.Tracks, t); }
        }

        [Fact]
        public void SearchArtistConstructor() {
            Search sArtist = new Search("dad", SearchType.ARTIST);

            Assert.NotEmpty(sArtist.Artists);
            foreach (Artist a in sArtist.Artists) { Assert.Single(sArtist.Artists, a); }
            Assert.Empty(sArtist.Albums);
            Assert.Empty(sArtist.Tracks);
        }

        [Fact]
        public void SearchAlbumConstructor() {
            Search sAlbum = new Search("dad", SearchType.ALBUM);

            Assert.Empty(sAlbum.Artists);
            Assert.NotEmpty(sAlbum.Albums);
            foreach (Album a in sAlbum.Albums) { Assert.Single(sAlbum.Albums, a); }
            Assert.Empty(sAlbum.Tracks);
            Assert.NotEmpty(sAlbum.Albums.First().Images); //getImages works
        }

        [Fact]
        public void SearchTrackConstructor() {
            Search sTrack = new Search("dad", SearchType.TRACK);

            Assert.NotEmpty(sTrack.Artists);
            Assert.NotEmpty(sTrack.Albums);
            Assert.NotEmpty(sTrack.Tracks);
            foreach (Track t in sTrack.Tracks) { Assert.Single(sTrack.Tracks, t); }
        }

        [Fact]
        public void SearchAddArtist() //Tests Search.addArtist
        {
            Search s = new Search("dad", SearchType.ALL);
            Artist a = new Artist("1234", "testArtist", s);

            Assert.False(s.Artists.Contains(a), "artist contained");
            s.AddArtist(a);
            Assert.True(s.Artists.Contains(a), "artist not contained");
            s.AddArtist(a);
            Assert.Single(s.Artists, a);
        }

        [Fact]
        public void SearchAddAlbum() //Tests Search.addArtist
        {
            Search s = new Search("dad", SearchType.ALL);
            Artist a = new Artist("1234", "testArtist", s);
            Image img1 = new Image(640, 640, "https://i.scdn.co/image/f8717f432506ab213c4de0c66d6ac24cd07ecf72");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/502bc1e1726e2594cd0045473e10d9166fa79dd8");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/d709f676d5c16db8941b1084b3ca348d35de04af");
            List<Image> imgs = new List<Image> { img1, img2, img3 };
            Album alb = new Album("1234", "testAlbum", "asdf", imgs, s);

            Assert.False(s.Albums.Contains(alb), "album contained");
            s.AddAlbum(alb);
            Assert.True(s.Albums.Contains(alb), "album not contained");
            s.AddAlbum(alb);
            Assert.Single(s.Albums, alb);
        }

        [Fact]
        public void SearchAddTrack() //Tests Search.AddTrack
        {
            Search s = new Search("dad", SearchType.ALL);
            List<Artist> artists = new List<Artist> { new Artist("1234", "testArtist", s) };
            Image img1 = new Image(640, 640, "https://i.scdn.co/image/f8717f432506ab213c4de0c66d6ac24cd07ecf72");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/502bc1e1726e2594cd0045473e10d9166fa79dd8");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/d709f676d5c16db8941b1084b3ca348d35de04af");
            List<Image> imgs = new List<Image> { img1, img2, img3 };
            Album alb = new Album("1234", "testAlbum", "asdf", imgs, s);
            Track t = new Track("1234", "testTrack", 10, 10, true, 10, alb, s, artists);

            Assert.False(s.Tracks.Contains(t), "track contained");
            s.AddTrack(t);
            Assert.True(s.Tracks.Contains(t), "track not contained");
            s.AddTrack(t);
            Assert.Single(s.Tracks, t);
        }

        [Fact]
        public void SearchNoDuplicatesInResults() //Tests for duplicates in Search results
        {
            Search s = new Search("dad", SearchType.ALL);
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
        public void SearchGetJObject() //Tests the getJObject method
        {
            JObject jo = Search.getJobject("https://api.spotify.com/v1/albums/3j3cgkuyo015dghNYhHnZJ");
            Assert.True(jo.HasValues, "JObject doesn't have values");
            Assert.True((string)jo["id"] == "3j3cgkuyo015dghNYhHnZJ", "id is not correct");
            Assert.True((string)jo["artists"].First()["external_urls"]["spotify"] == "https://open.spotify.com/artist/1vCWHaC5f2uS3yhpwWbIA6", "cannot find artist url via jobject");
        }
        #endregion

        #region WebAPILib.Artist
        [Fact]
        public void ArtistConstructor() {
            Search s = new Search("dad", SearchType.ALL);
            string id = "1234";
            string name = "testArtist";
            Artist a = new Artist(id, name, s);
            Assert.True(a.Name == name, "name not equal");
            Assert.True(a.ID == id, "id not equal");
        }

        [Fact]
        public void ArtistAddAlbum() // Tests Artist.AddAlbum
        {
            Search s = new Search("dad", SearchType.ALL);
            Artist a = new Artist("1234", "testArtist", s);
            Image img1 = new Image(640, 640, "https://i.scdn.co/image/f8717f432506ab213c4de0c66d6ac24cd07ecf72");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/502bc1e1726e2594cd0045473e10d9166fa79dd8");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/d709f676d5c16db8941b1084b3ca348d35de04af");
            List<Image> imgs = new List<Image> { img1, img2, img3 };
            Album alb = new Album("1234", "testAlbum", "asdf", imgs, s);

            Assert.False(a.Albums.Contains(alb), "album contained");
            a.AddAlbum(alb);
            Assert.True(a.Albums.Contains(alb), "album not contatined");
            a.AddAlbum(alb);
            Assert.Single(a.Albums, alb);
        }
        #endregion

        #region WebAPILib.Album
        [Fact]
        public void AlbumConstructor() {
            string id = "3j3cgkuyo015dghNYhHnZJ";
            string name = "testAlbum";
            string type = "asdf";
            Search s = new Search("dad", SearchType.ALL);
            Artist a = new Artist("1234", "testArtist", s);
            Artist b = new Artist("2345", "testArtist2", s);
            List<Artist> artists = new List<Artist> { a, b };
            Image img1 = new Image(640, 640, "https://i.scdn.co/image/f8717f432506ab213c4de0c66d6ac24cd07ecf72");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/502bc1e1726e2594cd0045473e10d9166fa79dd8");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/d709f676d5c16db8941b1084b3ca348d35de04af");
            List<Image> imgs = new List<Image> { img1, img2, img3 };
            Album alb = new Album(id, name, type, imgs, s);
            Album alb2 = new Album(id, name, type, imgs, s, artists);

            Assert.True(alb.ID == id, "id is not equal");
            Assert.True(alb.Name == name, "name is not equal");
            Assert.True(alb.AlbumType == type, "type is not equal");
            Assert.Equal(alb.Images, imgs);
            Assert.True(alb.SearchResult == s, "Searchres is not equal");

            Assert.True(alb2.ID == id, "id2 is not equal");
            Assert.True(alb2.Name == name, "name2 is not equal");
            Assert.True(alb2.AlbumType == type, "type2 is not equal");
            Assert.Equal(alb2.Images, imgs);
            Assert.True(alb2.SearchResult == s, "Searchres2 is not equal");
            Assert.Equal(alb2.Artists, artists);
        }


        [Fact]
        public void AlbumAddArtist() // Tests Album.AddArtist
        {
            Search s = new Search("dad", SearchType.ALL);
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
                Assert.False(alb.Artists.Contains(art), "Artist contained");
            }
            alb2.AddArtists(artists);
            foreach (Artist art in artists)
            {
                Assert.True(alb2.Artists.Contains(art), "Artist not contained");
            }
            foreach (Artist art in alb.Artists)
            {
                Assert.Single(alb.Artists, art);
            }
        }

        [Fact]
        public void AlbumAddTrack() // Tests Album.AddTrack
        {
            Search s = new Search("dad", SearchType.ALL);
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


            alb2.AddTrack(t);
            Assert.True(alb2.Tracks.Contains(t), "track not contained");
            alb2.AddTrack(t);
            Assert.Single(alb2.Tracks, t);
            foreach (Track track in alb.Tracks)
            {
                Assert.Single(alb.Tracks, track);
            }
        }

        [Fact]
        public void AlbumCache() {
            Search s = new Search("obliteration of the weak", SearchType.ALL);
            Image img1 = new Image(600, 600, "https://i.scdn.co/image/6885d1703f4f4fcbedd7beb231ecca8131de5683");
            Image img2 = new Image(300, 300, "https://i.scdn.co/image/c70d12c712e41ed4f532e4d190f3476380d0f708");
            Image img3 = new Image(64, 64, "https://i.scdn.co/image/cae856966342ec081a5dae800bb0efc8f8993612");
            List<Image> imgs = new List<Image> { img1, img2, img3 };
            Album alb = new Album("0hNtREj1dl7bKoWEz0XXMr", "Obliteration of the Weak", "album", imgs, s);

            Assert.False(alb.TracksCached, "tracksCached is true");
            Assert.False(alb.ArtistsCached, "ArtistsCached is true");
            Assert.NotEmpty(alb.Tracks);
            Assert.True(alb.Tracks.First().ID == "19pTAbMZmWsgGkYZ4v2TM1", "First track not identical");
            Assert.True(alb.Tracks.Last().ID == "6j9UgNQbcwZGrHkOJYMUdK", "Last track not identical");
            Assert.True(alb.Tracks.Count == 5, "Track count does not match");
            Assert.True(alb.TracksCached, "TracksCached not set");
            Assert.NotEmpty(alb.Artists);
            Assert.True(alb.Artists.First().ID == "40UIlN4YEByXy4ewEZmqXu", "artist not identical");
            Assert.True(alb.Artists.Count == 1, "artist count does not match");
            Assert.True(alb.ArtistsCached, "ArtistsCached not set");
        }
        #endregion
    }
}
