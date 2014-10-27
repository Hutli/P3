using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using SpotifyDotNet;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows;

namespace OpenPlaylistServer
{
    public class VoteModule : NancyModule
    {

        public VoteModule(IVoteService vs)
        {
            Get["/{trackUri}/{userId}/"] = parameters => {
                Application.Current.Dispatcher.BeginInvoke(  (Action)(() =>
                {
                    vs.Vote(parameters.userId, parameters.trackUri);
                }));
                
                return "Success";
            };
        }
    }
}
