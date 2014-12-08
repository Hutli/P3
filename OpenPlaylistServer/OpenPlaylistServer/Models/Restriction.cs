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
                return IntersperseRestrictions(TrackField.Artists);
            }
            set
            {
                var titlesParsed = ParseRestrictionUnits(value, TrackField.Artists);

                _restrictionUnits = AddRestrictionUnits(titlesParsed, TrackField.Artists);

                Predicate = UpdatePredicate(RestrictionType, _restrictionUnits);

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
                return IntersperseRestrictions(TrackField.Titles);
            }
            set
            {
                var titlesParsed = ParseRestrictionUnits(value, TrackField.Titles);

                _restrictionUnits = AddRestrictionUnits(titlesParsed, TrackField.Titles);

                Predicate = UpdatePredicate(RestrictionType, _restrictionUnits);

                OnPropertyChanged("Titles");
            }
        }

        private IEnumerable<RestrictionUnit> AddRestrictionUnits(IEnumerable<RestrictionUnit> toAdd, TrackField field)
        {
            return _restrictionUnits
                .Where(unit => unit.Field != field) // get anything other than title restrictionunits
                .Concat(toAdd); // append the titles parsed to the existing restrictionunits
        } 

        private IEnumerable<RestrictionUnit> ParseRestrictionUnits(String input, TrackField field)
        {
            var titlesParsed = Enumerable.Empty<RestrictionUnit>();
            // only if the new value is not whitespace, try to split it up
            if (!string.IsNullOrWhiteSpace(input))
            {
                // parse titles, and construct restrictionunits
                titlesParsed = input.Split(',')
                .Select(str => new RestrictionUnit(field, str));
            }

            return titlesParsed;
        }

        public RestrictionType RestrictionType
        {
            get { return _restrictionType; }
            private set
            {
                _restrictionType = value;
                OnPropertyChanged("RestrictionType");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private RestrictionType _restrictionType;
        private IEnumerable<RestrictionUnit> _restrictionUnits;
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

            Predicate = filter;
            _startTime = timeStart;
            _endTime = timeEnd;
            _restrictionType = restrictionType;
            _restrictionUnits = restrictionUnits;
        }

        private string IntersperseRestrictions(TrackField field)
        {
            // find all restrictions of type field, and intersperse the elements with ','
            var ex1 = _restrictionUnits.Where(unit => unit.Field == field);
            return string.Join(",", ex1.Select(unit => unit.FieldValue));
        }

        private static Func<Track, bool> UpdatePredicate(RestrictionType restrictionType, IEnumerable<RestrictionUnit> restrictionUnits)
        {
            Func<Track, bool> filter = t => true;
            // chain all restrictionunits together
            foreach (var restrictionUnit in restrictionUnits)
            {
                if(string.IsNullOrWhiteSpace(restrictionUnit.FieldValue))
                    continue;
                var prevFilter = filter;
                RestrictionUnit unit = restrictionUnit;
                Func<Track, bool> currentFilter = t =>
                {
                    switch (unit.Field)
                    {
                        case TrackField.Titles:
                            return t.Name.ToLower().Contains(unit.FieldValue);

                        case TrackField.Artists:
                            return t.Album.Artists.Any(a => a.Name.ToLower().Contains(unit.FieldValue));
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                };

                filter = t => prevFilter(t) && currentFilter(t);
            }

            if (restrictionType == RestrictionType.BlackList)
            {
                Func<Track, bool> filter1 = filter;
                filter = t => !filter1(t);
            }

            return filter;
        }

        public Func<Track, bool> Predicate { get; private set; }

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
