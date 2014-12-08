using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace WebAPI
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Track : SpotifyObject, INotifyPropertyChanged
    {
        [JsonConstructor]
        public Track(string id, string name, int duration, bool isExplicit, int trackNumber, string isrc, string previewURL, Album album)
            : base(id, name)
        {
            Duration = duration;
            IsExplicit = isExplicit;
            TrackNumber = trackNumber;
            ISRC = isrc;
            PreviewURL = previewURL;
            Album = album;
            //IsFiltered = false;
        }


        public Track()
        {

        }


        private int _tScore;
        private int _pScore;

        public int TScore
        {
            get
            {
                return _tScore;
            }
            set
            {
                _tScore = value;
                OnPropertyChanged("TScore");
            }
        }

        public int PScore
        {
            get { return _pScore; }
            set
            {
                _pScore = value;
                OnPropertyChanged("PScore");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string pName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pName));
            }
        }

        public string ISRC { get; set; }

        public bool IsFiltered { get; set; }

        public string PreviewURL { get; set; }

        public int Duration { get; set; }

        public int CurrentDurationStep { get; set; }

        public bool IsSelected { get; set; }

        public bool IsExplicit { get; set; }

        public int TrackNumber { get; set; }

        public int TotalScore
        {
            get { return _pScore + _tScore; }
        }

        [JsonProperty]
        public Album Album { get; set; }

        public override string URI { get { return "spotify:track:" + Id; } }

        public override string ToString() { return string.Format("{0} on {1}", Name, Album); }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Track))
            {
                return (((Track)obj).Id == Id || ((Track)obj).ISRC == ISRC) && ((Track)obj).TotalScore == TotalScore;
            }
            return false;
        }

        public bool Equals(string id, string isrc)
        {
            return (Id == id || ISRC == isrc);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

