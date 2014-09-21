using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace jsonTest {
    class Program {
        static void Main(string[] args) {
            WebClient client = new WebClient();
            string content = client.DownloadString("https://api.spotify.com/v1/search?q=roadhouse&type=track");
            pagingObj deserializedProduct = JsonConvert.DeserializeObject<pagingObj>(content);
            Console.WriteLine(deserializedProduct.key);
            Console.ReadKey();
        }
    }

    class pagingObj {
        public int key;
        public string href;
        public object[] items;
        public int limit;
        public string next;
        public int offset;
        public string previous;
        public int total;
    }

    class Track {


    }

    class Album {


    }

    class Artist {


    }
}
