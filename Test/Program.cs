using System;
using System.Linq;
using WebAPILib;
namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime timer = DateTime.Now;
            Search trackSearch = new Search("DAD", SearchType.Track);
            Console.WriteLine((DateTime.Now - timer).TotalMilliseconds);
            timer = DateTime.Now;
            Search allSearch = new Search("DAD");
            Console.WriteLine((DateTime.Now - timer).TotalMilliseconds);
            for (int i = 0; i < trackSearch.Tracks.Count(); i++)
            {
                Console.WriteLine("{0} | {1}", trackSearch.Tracks[i], allSearch.Tracks[i]);
            }

            Console.WriteLine(allSearch.Tracks[0].Album.Name);
            Console.WriteLine(allSearch.Tracks[0].Album.Artists[0].Name);

            Console.WriteLine("DONE");
            Console.ReadLine();
        }
    }
}
