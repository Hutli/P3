using System;
using System.ComponentModel;
using System.Linq;
using WebAPI;

namespace OpenPlaylistServer.Models
{
    public class Restriction : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private Func<Track,bool> _predicate;

        private TimeSpan _timeStart;
        private TimeSpan _timeEnd; // Maybe duration instead

        public string Time
        {
            get { return _timeStart.ToString("G") + " - " + _timeEnd.ToString("G"); }
        }

        public Restriction(String name, TimeSpan timeStart, TimeSpan timeEnd, params RestrictionUnit[] restrictionUnits)
        {
            Name = name;
            Func<Track, bool> filter = t => true;
            // chain all restrictionunits together
            foreach (var restrictionUnit in restrictionUnits)
            {
                var prevFilter = filter;
                RestrictionUnit unit = restrictionUnit;
                Func<Track, bool> currentFilter = t =>
                {
                    switch (unit.Field)
                    {
                        case TrackField.Name:
                            return t.Name.Contains(unit.FieldValue);
                            
                        case TrackField.Artists:
                            return t.Album.Artists.Any(a => a.Name.Contains(unit.FieldValue));
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                };

                if(unit.Type == RestrictionType.BlackList) {
                        Func<Track, bool> filter1 = currentFilter;
                        currentFilter = t => !filter1(t);
                }

                filter = t => prevFilter(t) && currentFilter(t);
            }
            
            _predicate = filter;
            _timeStart = timeStart;
            _timeEnd = timeEnd;
        }

        public Func<Track, bool> Predicate
        {
            get { return _predicate; }
            private set { _predicate = value; }
        }

        //ToStringPredicate

        void OnPropertyChanged(string pName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pName));
            }
        } 
    }
}
