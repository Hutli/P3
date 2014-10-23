using System;
using System.Collections.Generic;
using System.Text;

namespace OpenPlaylistApp
{
    public class ConnectionToServerFaultedException : Exception
    {
        public ConnectionToServerFaultedException() {
        }

        public ConnectionToServerFaultedException(string message) : base(message) {
        }

        public ConnectionToServerFaultedException(string message, Exception inner) : base(message,inner){
        }
    }
}
