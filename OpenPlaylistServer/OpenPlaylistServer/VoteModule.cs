using System;
using System.Linq;
using Nancy;
using System.Windows;
using Newtonsoft.Json;
using WebAPILib;

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
                Search search = searchService.Search(parameters.query);
                var t = search.Tracks;
                return JsonConvert.SerializeObject(t, Formatting.Indented);
            };
        }
    }
}