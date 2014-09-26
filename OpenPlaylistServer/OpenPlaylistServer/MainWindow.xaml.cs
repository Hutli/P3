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

namespace OpenPlaylistServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Session session = Session.Instance;

        private static byte[] appkey = File.ReadAllBytes("spotify_appkey.key");
        
        private static NAudio.Wave.WaveFormat activeFormat;
        private static NAudio.Wave.BufferedWaveProvider sampleStream;
        private static NAudio.Wave.WaveOut waveOut;

        public MainWindow(){
            InitializeComponent();

            session.OnLogIn += OnLogIn;
            session.MusicDelivery += OnRecieveData;
            session.Init(appkey);
            
                //34AKPAKCRE77K
            session.Login("jensstaermose@hotmail.com", "34AKPAKCRE77K");
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
            List<Track> tracks = session.FromLink("spotify:track:7eWYXAP87TFfF7fn2LEL1b");
            session.Play(tracks.First());
        }

        private void StopButton_Click(object sender, RoutedEventArgs e) {
            session.Stop(); //Hammertime
            //Don't
            waveOut.Stop(); //Believin'
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
