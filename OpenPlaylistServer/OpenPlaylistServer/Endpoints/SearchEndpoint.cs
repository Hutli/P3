using System.Collections.Generic;
using System.Linq;
using Nancy;
using Newtonsoft.Json;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer.Endpoints
{
    public class SearchEndpoint : NancyModule
    {
        private IEnumerable<Restriction> restrictions = new List<Restriction>()
        {
            new Restriction(track => track.Album.Artists.All(artist => !artist.Name.Contains("Bieber"))),
            new Restriction(track => track.Name != "Still Alive")
        };

        public SearchEndpoint(ISearchService searchService, IFilterService filterService)
        {
            Get["/search/{query}"] = parameters =>
            {
                var tracks = searchService.Search(parameters.query);
                filterService.FilterTracks(tracks, restrictions);
                return JsonConvert.SerializeObject(tracks, Formatting.Indented);
            };
        }
    }
}
