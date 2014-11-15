using System;
using Nancy;
using System.Windows;

namespace OpenPlaylistServer
{
    public class VoteModule : NancyModule
    {

        public VoteModule(IVoteService vs)
        {
            Get["/{trackUri}/{userId}/"] = parameters => {
                Application.Current.Dispatcher.BeginInvoke(  (Action)(() => vs.Vote(parameters.userId, parameters.trackUri)));
                
                return "Success";
            };
        }
    }
}
