using System.Collections.Generic;
using System.Linq;
using Nancy;
using Newtonsoft.Json;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;
using WebAPI;

namespace OpenPlaylistServer.Endpoints
{
    public class SearchEndpoint : NancyModule
    {
        private IEnumerable<Restriction> restrictions = new List<Restriction>()
        {
            new Restriction(track => track.Album.Artists.All(artist => !artist.Name.Contains("Bieber"))),
            //new Restriction(track => track.Name != "Still Alive")
        };

        public SearchEndpoint(ISearchService searchService, IFilterService filterService)//, IPlaylistService playlistService)
        {
            Get["/search/{query}"] = parameters => {
                var tracks = searchService.Search(parameters.query).Result;
                filterService.FilterTracks(tracks, restrictions);
                //foreach (Track t in tracks)
                //{
                //    Track tmpTrack = playlistService.Tracks.FirstOrDefault(p => p.Equals(t));
                //    if (tmpTrack != null)
                //    {
                //        t.TScore = tmpTrack.TotalScore;
                //    }
                //}
                return JsonConvert.SerializeObject(tracks, Formatting.Indented);
            };

            Get["/search/{query}/{offset}"] = parameters =>
            {
                var tracks = searchService.Search(parameters.query, parameters.offset).Result;
                filterService.FilterTracks(tracks, restrictions);
                //foreach (Track t in tracks)
                //{
                //    Track tmpTrack = playlistService.Tracks.FirstOrDefault(p => p.Equals(t));
                //    if (tmpTrack != null)
                //    {
                //        t.TScore = tmpTrack.TotalScore;
                //    }
                //}
                return JsonConvert.SerializeObject(tracks, Formatting.Indented);
            };
        }
    }
}
