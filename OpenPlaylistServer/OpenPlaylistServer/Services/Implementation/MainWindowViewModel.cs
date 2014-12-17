using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using OpenPlaylistServer.Models;
using OpenPlaylistServer.Services.Interfaces;
using WebAPI;

namespace OpenPlaylistServer.Services.Implementation
{
    internal class MainWindowViewModel : IMainWindowViewModel
    {
        private bool _trackEndedAlreadyCalled;
        private readonly IHistoryService _historyService;
        private readonly IPlaybackService _playbackService;
        private readonly IPlaylistService _playlistService;
        private readonly IRestrictionService _restrictionService;
        private readonly IUserService _userService;

        public MainWindowViewModel(IPlaylistService playlistService, IUserService userService, IPlaybackService playbackService, IHistoryService historyService, IRestrictionService restrictionService)
        {
            _playlistService = playlistService;
            _userService = userService;
            _playbackService = playbackService;
            _historyService = historyService;
            _restrictionService = restrictionService;
        }

        private ObservableCollection<User> Users
        {
            get { return _userService.Users; }
        }

        public ObservableCollection<Track> Tracks
        {
            get { return _playlistService.Tracks; }
        }

        public ObservableCollection<Track> History
        {
            get { return _historyService.Tracks; }
        }

        public ObservableCollection<Restriction> Restrictions
        {
            get { return _restrictionService.Restrictions; }
        }

        public void AddRestriction(Restriction restriction) { _restrictionService.AddRestriction(restriction); }
        public void RemoveRestriction(Restriction restriction) { _restrictionService.RemoveRestriction(restriction); }

        public void TrackEnded()
        {
            Console.WriteLine("Track ended called from: " + Thread.CurrentThread.ManagedThreadId);
            // TrackEnded is called from libspotify running in a different thread than the UI thread.

            if(_trackEndedAlreadyCalled)
            {
                Console.WriteLine("Avoided calling track ended twice");
                return;
            }
            _trackEndedAlreadyCalled = true;
            RootDispatcherFetcher.RootDispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                                                                                                   {
                                                                                                       try
                                                                                                       {
                                                                                                           var currentPlaying = _playbackService.GetCurrentPlaying();
                                                                                                           if(currentPlaying != null)
                                                                                                               _historyService.Add(currentPlaying);

                                                                                                           var next = _playlistService.NextTrack();
                                                                                                           Console.WriteLine("Next track will be " + next);
                                                                                                           if(next != null)
                                                                                                           {
                                                                                                               _playbackService.Stop();
                                                                                                               _playbackService.Play(next);
                                                                                                           }

                                                                                                           //reset trackEndedCalled flag
                                                                                                           _trackEndedAlreadyCalled = false;
                                                                                                           Console.WriteLine("Track ended can now be called again");
                                                                                                       } catch(Exception e)
                                                                                                       {
                                                                                                           Console.WriteLine(e);
                                                                                                           throw;
                                                                                                       }
                                                                                                   }));
        }

        public void PlayButtonClicked() { TrackEnded(); }
        public void StopButtonClicked() { _playbackService.Stop(); }

        public void RemoveTrack_Click(Track track)
        {
            Tracks.Remove(track);

            var users = Users.Where(u => u.Vote != null && Equals(u.Vote.Track, track));
            foreach(var u in users)
                u.Vote = null;
        }

        public void MoveUp_Click(Track track)
        {
            var largerTracks = Tracks.Where(t => t.TotalScore >= track.TotalScore && !Equals(t, track));
            if(!largerTracks.Any())
                return;
            var min = largerTracks.Min(t => t.TotalScore);
            track.PScore = min + 1 - track.TScore;
        }

        public void MoveDown_Click(Track track)
        {
            var smallerTracks = Tracks.Where(t => t.TotalScore <= track.TotalScore && !Equals(t, track));
            if(!smallerTracks.Any())
                return;
            var max = smallerTracks.Max(t => t.TotalScore);
            track.PScore = max - 1 - track.TScore;
        }
    }
}