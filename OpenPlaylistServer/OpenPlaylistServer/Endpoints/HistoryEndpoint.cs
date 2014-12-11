﻿using System.Linq;
using Nancy;
using Newtonsoft.Json;
using OpenPlaylistServer.Services.Interfaces;
using WebAPI;

namespace OpenPlaylistServer.Endpoints
{
    public class HistoryEndpoint : NancyModule
    {
        public HistoryEndpoint(IHistoryService historyService)
        {
            var tracks = historyService.Tracks;
            var ordered = tracks.Reverse();
            
            Get["history"] = e => JsonConvert.SerializeObject(ordered, Formatting.Indented);
        }
    }
}
