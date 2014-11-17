using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                Console.WriteLine(string.Format("{0} | {1}", trackSearch.Tracks[i], allSearch.Tracks[i]));
            }

            Console.WriteLine("DONE");
            Console.ReadLine();
        }
    }
}
