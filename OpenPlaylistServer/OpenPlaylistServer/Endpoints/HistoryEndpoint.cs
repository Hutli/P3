using Nancy;
using Newtonsoft.Json;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer.Endpoints
{
    public class HistoryEndpoint : NancyModule
    {
        public HistoryEndpoint(IHistoryService historyService)
        {
            var tracks = historyService.Tracks;

            Get["history"] = e => JsonConvert.SerializeObject(tracks, Formatting.Indented);
        }
    }
}