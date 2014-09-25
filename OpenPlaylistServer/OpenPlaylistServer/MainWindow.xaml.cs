using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SpotifyDotNet;
using System.IO;
using NAudio;

namespace OpenPlaylistServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Session session = Session.Instance;
        static string loginStatus = "Not logged in";
        
        private static byte[] appkey = File.ReadAllBytes("spotify_appkey.key");
        

        private static NAudio.Wave.WaveFormat activeFormat;
        private static NAudio.Wave.BufferedWaveProvider sampleStream;
        private static NAudio.Wave.WaveOut waveOut;

        public MainWindow()
        {
            InitializeComponent();

            session.LoggedIn += LoggedIn;
            session.MusicDelivery += OnRecieveData;
            session.Init(appkey, sampleStream);
            session.Login("jensstaermose@hotmail.com", "34AKPAKCRE77K");

           
        }

        void LoggedIn()
        {
            //LoggedInStatus.Content = "Logged in";
            //loginStatus = "Logged in";
            //Console.WriteLine("logged in");
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            List<Track> tracks = session.FromLink("spotify:track:7eWYXAP87TFfF7fn2LEL1b");
            session.Play(tracks.First());
        }

        private static void OnRecieveData(int sample_rate, int channels, byte[] frames)
        {

            if (activeFormat == null)
                activeFormat = new NAudio.Wave.WaveFormat(sample_rate, 16, channels);

            if (sampleStream == null)
            {
                sampleStream = new NAudio.Wave.BufferedWaveProvider(activeFormat);
                sampleStream.DiscardOnBufferOverflow = true;
                sampleStream.BufferLength = 3000000;
            }

            if (waveOut == null)
            {

                waveOut = new NAudio.Wave.WaveOut();
                waveOut.Init(sampleStream);
                waveOut.Play();
            }

            sampleStream.AddSamples(frames, 0, frames.Length);
        }
    }
}
