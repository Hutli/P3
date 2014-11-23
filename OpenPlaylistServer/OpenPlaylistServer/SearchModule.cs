using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Newtonsoft.Json;

namespace OpenPlaylistServer
{
    public class SearchModule : NancyModule
    {
        private IEnumerable<Restriction> restrictions = new List<Restriction>()
        {
            new Restriction(track => track.Album.Artists.All(artist => !artist.Name.Contains("Bieber")))
        };

        public SearchModule(ISearchService searchService, IFilterService filterService)
        {
            Get["/search/{query}"] = parameters =>
            {
                var tracks = searchService.Search(parameters.query);
                tracks = filterService.FilterTracks(tracks, restrictions);
                return JsonConvert.SerializeObject(tracks, Formatting.Indented);
            };
        }
    }
}
