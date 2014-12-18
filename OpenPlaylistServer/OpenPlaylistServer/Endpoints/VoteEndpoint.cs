using Nancy;
using OpenPlaylistServer.Services.Interfaces;

namespace OpenPlaylistServer.Endpoints {
    public class VoteEndpoint : NancyModule {
        public VoteEndpoint(IVoteService vs) {
            Get["/vote/{trackUri}/{userId}"] = parameters => {
                if(vs.Vote(parameters.userId, parameters.trackUri))
                    return "Success";
                return "Failure";
            };
        }
    }
}