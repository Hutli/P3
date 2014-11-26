using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using System.Windows;
using Newtonsoft.Json;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer
{
    public class VoteEndpoint : NancyModule
    {
        public VoteEndpoint(IVoteService vs)
        {
            Get["/vote/{trackUri}/{userId}"] = parameters => {
                Application.Current.Dispatcher.BeginInvoke(  (Action)(() => vs.Vote(parameters.userId, parameters.trackUri)));
                
                return "Success";
            };
        }
    }
}