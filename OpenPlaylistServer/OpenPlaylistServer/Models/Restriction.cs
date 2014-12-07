using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WebAPI;

namespace OpenPlaylistServer.Models
{
    public class Restriction : INotifyPropertyChanged
    {

        /// <summary>
        /// Artists are seperated using comma
        /// </summary>
        public String Artists
        {
            get
            {
                
                // find all artists restrictions, and intersperse the elements with ','
                var ex1 =_restrictionUnits.Where(unit => unit.Field == TrackField.Artists);
                return string.Join(",", ex1.Select(unit => unit.FieldValue));
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }
                // parse artists, and construct restrictionunits
                var artistsParsed = value.Split(',')
                    .Select(str => new RestrictionUnit(TrackField.Artists, str));

                _restrictionUnits = _restrictionUnits
                    .Where(unit => unit.Field != TrackField.Artists) // get anything other than artist restrictionunits
                    .Concat(artistsParsed) // append the artists parsed to the existing restrictionunits
                    .ToArray();

                UpdatePredicate(RestrictionType, _restrictionUnits);

                OnPropertyChanged("Artists");

            }
        }

        /// <summary>
        /// Titles are seperated using comma
        /// </summary>
        public String Titles
        {
            get
            {

                // find all title restrictions, and intersperse the elements with ','
                var ex1 = _restrictionUnits.Where(unit => unit.Field == TrackField.Titles);
                return string.Join(",", ex1.Select(unit => unit.FieldValue));
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }
                // parse artists, and construct restrictionunits
                var titlesParsed = value.Split(',')
                    .Select(str => new RestrictionUnit(TrackField.Titles, str));

                _restrictionUnits = _restrictionUnits
                    .Where(unit => unit.Field != TrackField.Titles) // get anything other than artist restrictionunits
                    .Concat(titlesParsed) // append the artists parsed to the existing restrictionunits
                    .ToArray();

                UpdatePredicate(RestrictionType, _restrictionUnits);

                OnPropertyChanged("Titles");
            }
        }

        public RestrictionType RestrictionType
        {
            get { return _restrictionType; }
            set
            {
                _restrictionType = value;
                OnPropertyChanged("RestrictionType");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private Func<Track,bool> _predicate;

        private RestrictionType _restrictionType;
        private RestrictionUnit[] _restrictionUnits;
        private string _name;
        private string _artists;
        private DateTime _startTime;
        private DateTime _endTime;

        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnPropertyChanged("StartTime");
                OnPropertyChanged("IsActive");
            }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                OnPropertyChanged("EndTime");
                OnPropertyChanged("IsActive");
            }
        }

        public bool IsActive
        {
            get
            {
                var now = DateTime.Now.TimeOfDay;
                var startTimeCompare = StartTime.TimeOfDay.CompareTo(now);
                var endTimeCompare = EndTime.TimeOfDay.CompareTo(now);
                if (startTimeCompare <= 0 && endTimeCompare >= 0)
                {
                    return true;
                }
                return false;
            }
        }

        public Restriction(DateTime timeStart, DateTime timeEnd, RestrictionType restrictionType, params RestrictionUnit[] restrictionUnits)
        {
            var filter = UpdatePredicate(restrictionType, restrictionUnits);

            _predicate = filter;
            _startTime = timeStart;
            _endTime = timeEnd;
            _restrictionType = restrictionType;
            _restrictionUnits = restrictionUnits;
        }

        private static Func<Track, bool> UpdatePredicate(RestrictionType restrictionType, RestrictionUnit[] restrictionUnits)
        {
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
                        case TrackField.Titles:
                            return t.Name.Contains(unit.FieldValue);

                        case TrackField.Artists:
                            return t.Album.Artists.Any(a => a.Name.Contains(unit.FieldValue));
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                };

                if (restrictionType == RestrictionType.BlackList)
                {
                    Func<Track, bool> filter1 = currentFilter;
                    currentFilter = t => !filter1(t);
                }

                filter = t => prevFilter(t) && currentFilter(t);
            }
            return filter;
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
