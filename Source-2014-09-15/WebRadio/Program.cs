using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using WebRadio.API;
using libspotifydotnet;
using System.Runtime.InteropServices;
using System.Threading;
using NAudio.Wave;

namespace WebRadio {
    public delegate void SearchCompleteHandler(SearchResults results);
    public delegate void NotifyMainHandler(IntPtr session);
    public delegate void MusicDeliveryHandler(libspotify.sp_audioformat audioFormat, byte[] frames);

    public class Program {

        private static byte[] appkey;
        
        private static WaveFormat activeFormat;
        public static BufferedWaveProvider sampleStream;
        private static WaveOut waveOut;


        static void Main(string[] args) {
            appkey = File.ReadAllBytes("spotify_appkey.key");

            Session.LoggedIn += new Action(LoggedIn);
            Session.SearchComplete += new SearchCompleteHandler(searchCompleteTest);
            Session.MusicDelivery += new MusicDeliveryHandler(musicDeliveryTest);
            Session.TrackEnded += new Session.TrackEndedDelegate((session) => {
                waveOut.Pause();
                Console.WriteLine("Track Ended, so paused");
            });
            
            
            Session.Init(appkey);
            Session.Login("jensstaermose@hotmail.com", "34AKPAKCRE77K");
            

            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        Session.ProcessEvents();
            //    }
            //});
            
            Console.WriteLine("At readline");
            Console.ReadLine();
        }

        private static void LoggedIn()
        {
            Console.WriteLine("Logged in");

            //Session.Search.BeginSearchOnQuery("Even My Dad Does Sometimes");
            //List<Track> track0 = Session.FromLink("spotify:track:7eWYXAP87TFfF7fn2LEL1b"); // Single Track
            List<Track> track0 = Session.FromLink("spotify:track:43lVx5Sh75Yh8yS0rAebsN"); // Playlist
            Session.Player.Play(track0.FirstOrDefault());
        }

        private static void searchCompleteTest(SearchResults results)
        {
            //Track track = results.Tracks.ElementAt(0);
            //IntPtr spLinkPtr = libspotify.sp_link_create_from_track(track.trackPtr, 0);

            //IntPtr buffer = Marshal.AllocHGlobal(1024);

            //libspotify.sp_link_as_string(spLinkPtr, buffer, 1024);

            //String trackString = Marshal.PtrToStringAnsi(buffer);
            //Console.WriteLine(trackString);

            Session.Player.Play(results.Tracks.ElementAt(0));
            //Track track0 = Session.FromLink("spotify:track:1GVvVyhqI5rbkCrPvAGgH5");
            //Session.Player.Play(track0);
            //foreach (var track in results.Tracks)
            //{
            //    Console.WriteLine(track.Name);
            //}
            
        }

        private static void musicDeliveryTest(libspotify.sp_audioformat audioFormat, byte[] frames)
        {
           
            if (activeFormat == null)
                activeFormat = new WaveFormat(audioFormat.sample_rate, 16, audioFormat.channels);

            if (sampleStream == null)
            {
                sampleStream = new BufferedWaveProvider(activeFormat);
                sampleStream.DiscardOnBufferOverflow = true;
                sampleStream.BufferLength = 3000000;
            }

            if (waveOut == null)
            {

                waveOut = new WaveOut();
                waveOut.Init(sampleStream);
                waveOut.Play();
            }

            sampleStream.AddSamples(frames, 0, frames.Length);
            Console.WriteLine("Buffered duration " + sampleStream.BufferedDuration);
            Console.WriteLine("Buffered bytes (BufferedBytes) " + sampleStream.BufferedBytes);
        }

    }
}
