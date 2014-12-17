using Nancy;
using Newtonsoft.Json;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer.Endpoints
{
    public class SearchEndpoint : NancyModule
    {
        public SearchEndpoint(ISearchService searchService, IRestrictionService restrictionService)
            //, IPlaylistService playlistService)
        {
            Get["/search/{query}"] = parameters =>
                                     {
                                         var tracks = searchService.Search(parameters.query).Result;
                                         restrictionService.RestrictTracks(tracks);
                                         return JsonConvert.SerializeObject(tracks, Formatting.Indented);
                                     };

            Get["/search/{query}/{offset}"] = parameters =>
                                              {
                                                  var tracks = searchService.Search(parameters.query, parameters.offset).Result;
                                                  restrictionService.RestrictTracks(tracks);
                                                  return JsonConvert.SerializeObject(tracks, Formatting.Indented);
                                              };
        }
    }
}