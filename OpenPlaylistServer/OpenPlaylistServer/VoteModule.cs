using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using System.Windows;
using Newtonsoft.Json;

namespace OpenPlaylistServer
{
    public class VoteModule : NancyModule
    {
        private IEnumerable<Restriction> restrictions = new List<Restriction>()
        {
            new Restriction(track => track.Album.Artists.All(artist => !artist.Name.Contains("Bieber")))
        };

        public VoteModule(IVoteService vs, ISearchService searchService, IFilterService filterService)
        {
            Get["/{trackUri}/{userId}"] = parameters => {
                Application.Current.Dispatcher.BeginInvoke(  (Action)(() => vs.Vote(parameters.userId, parameters.trackUri)));
                
                return "Success";
            };

            Get["/search/{query}"] = parameters =>
            {
                var tracks = searchService.Search(parameters.query);
                tracks = filterService.FilterTracks(tracks, restrictions);
                return JsonConvert.SerializeObject(tracks, Formatting.Indented);
            };
        }
    }
}