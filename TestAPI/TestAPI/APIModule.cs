using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace TestAPI
{
    public class APIModule : NancyModule
    {
        public static Dictionary<String, int> votes = new Dictionary<string,int>();
        public APIModule()
        {
            Get["/"] = parameters => "Server is running";
            Get["/vote/{id}"] = parameters =>
            {
                if (votes.ContainsKey(parameters.id))
	            {
		            votes[parameters.id] += 1;
	            } else
	            {
                    votes[parameters.id] = 1;
	            }
                return votes[parameters.id] + " OK total votes for: " + parameters.id;
            };
        }
    }
}