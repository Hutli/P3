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
using System.ComponentModel;
using Nancy.Hosting.Self;

namespace OpenPlaylistServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Session session = Session.Instance;
        private static byte[] appkey = OpenPlaylistServer.Properties.Resources.spotify_appkey;
        
        private static NAudio.Wave.WaveFormat activeFormat;
        private static NAudio.Wave.BufferedWaveProvider sampleStream;
        private static NAudio.Wave.WaveOut waveOut;

        public MainWindow(){
            InitializeComponent();

            var hostConfig = new HostConfiguration();
            hostConfig.UrlReservations.CreateAutomatically = true;


            var host = new NancyHost(hostConfig, new Uri("http://localhost:1234"));
            host.Start();

            session.OnLogIn += OnLogIn;
            session.MusicDelivery += OnRecieveData;
            
                //34AKPAKCRE77K
            session.Login("jensstaermose@hotmail.com", "34AKPAKCRE77K", appkey);
        }

        void OnLogIn(LoginState loginState)
        {
            if (loginState == LoginState.OK)
            {
                Dispatcher.Invoke((Action)(() => LoggedInStatus.Content = "Succesfully logged in"));
            }
            else
            {
                Dispatcher.Invoke((Action)(() => {
                    LoggedInStatus.Content = "Error: " + loginState.ToString();
                    PlayButton.IsEnabled = false;
                }));
            }
            
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Track tracks = session.TrackFromLink("spotify:track:7eWYXAP87TFfF7fn2LEL1b");
            session.Play(tracks);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e) {
            session.Stop(); //Hammertime
            //Don't
            waveOut.Stop(); //Believin'
            waveOut = null;
            activeFormat = null;
            sampleStream = null;
        }

        private void OnRecieveData(int sample_rate, int channels, byte[] frames)
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
            session.BufferedBytes = sampleStream.BufferedBytes;
            session.BufferedDuration = sampleStream.BufferedDuration;
            sampleStream.AddSamples(frames, 0, frames.Length);
        }

    }
}
