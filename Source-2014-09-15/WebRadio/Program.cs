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
    public delegate void LoggedInHandler(IntPtr session, libspotify.sp_error error);
    public delegate void SearchCompleteHandler(SearchResults results);
    public delegate void NotifyMainHandler(IntPtr session);
    public delegate void MusicDeliveryHandler(libspotify.sp_audioformat audioFormat, byte[] frames);

    public class Program {

        private static ManualResetEvent _searchCompleteSignal;
        private static byte[] appkey;
        

        private static BinaryWriter _binaryWriter;
        private static WaveFormat activeFormat;
        public static BufferedWaveProvider sampleStream;
        private static WaveOut waveOut;


        static void Main(string[] args) {
            appkey = File.ReadAllBytes("spotify_appkey.key");

            _binaryWriter = new BinaryWriter(File.Open("test.dat", FileMode.Create));

            
            

            Session.LoggedIn += new LoggedInHandler(loginTest);
            Session.SearchComplete += new SearchCompleteHandler(searchCompleteTest);
            Session.MusicDelivery += new MusicDeliveryHandler(musicDeliveryTest);
            Session.TrackEnded += new Session.TrackEndedDelegate((session) => {
                waveOut.Pause();
                Console.WriteLine("Track Ended, so paused");
            });
            
            try
            {
                Session.Init(appkey);
                Session.Login("jensstaermose@hotmail.com", "pass");
                Session.Search.BeginSearchOnQuery("parking lot skit eminem");

                _searchCompleteSignal = new ManualResetEvent(false);
            }
            catch {}

            

            while (true)
            {
                do
                {
                    libspotify.sp_session_process_events(Session._sessionPtr, out Session._nextTimeout);
                } while (Session._nextTimeout == 0);
            }
            Console.WriteLine("At readline");
            Console.ReadLine();
        }

        private static void loginTest(IntPtr session, libspotify.sp_error error)
        {
            Console.WriteLine("Login:" + error);
        }

        private static void searchCompleteTest(SearchResults results)
        {
            Session.Player.Play(results.Tracks.ElementAt(0));
            //foreach (var track in results.Tracks)
            //{
            //    Console.WriteLine(track.Name);
            //}
            
        }

        private static void musicDeliveryTest(libspotify.sp_audioformat audioFormat, byte[] frames)
        {
            ////_binaryWriter.Write(frames);
            if (sampleStream != null)
            {
                //sampleStream.ClearBuffer();
            }
           
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
