using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string searchString = Console.ReadLine();
            List<Track> testTracks = WebAPIMethods.Search(searchString, 50);
            Console.WriteLine(testTracks[0].Album.Artists[0].Name);
            foreach (Track t in testTracks)
            {
                Console.WriteLine(t);
            }
            Console.ReadLine();
        }
    }
}
