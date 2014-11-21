using System;
using System.Linq;
using Nancy;
using System.Windows;
using Newtonsoft.Json;

namespace OpenPlaylistServer
{
    public class VoteModule : NancyModule
    {

        public VoteModule(IVoteService vs, ISearchService searchService)
        {
            Get["/{trackUri}/{userId}/"] = parameters => {
                Application.Current.Dispatcher.BeginInvoke(  (Action)(() => vs.Vote(parameters.userId, parameters.trackUri)));
                
                return "Success";
            };

            Get["/search/{query}"] = parameters =>
            {
                var tracks = searchService.Search(parameters.query);
                return JsonConvert.SerializeObject(tracks, Formatting.Indented);
            };
        }
    }
}