using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WebAPI;

namespace OpenPlaylistServer.Models {
    public class Restriction : INotifyPropertyChanged {
        private DateTime _endTime;
        private RestrictionType _restrictionType;
        private IEnumerable<RestrictionUnit> _restrictionUnits;
        private DateTime _startTime;

        public Restriction(DateTime timeStart,
                           DateTime timeEnd,
                           RestrictionType restrictionType,
                           params RestrictionUnit[] restrictionUnits) {
            var filter = UpdatePredicate(restrictionType, restrictionUnits);

            Predicate = filter;
            _startTime = timeStart;
            _endTime = timeEnd;
            _restrictionType = restrictionType;
            _restrictionUnits = restrictionUnits;
        }

        /// <summary>
        ///     Artists are seperated using comma
        /// </summary>
        public String Artists {
            get {
                // find all artists restrictions, and intersperse the elements with ','
                return IntersperseRestrictions(TrackField.Artists);
            }
            set {
                var artistsParsed = ParseRestrictionUnits(value, TrackField.Artists);

                _restrictionUnits = AddRestrictionUnits(artistsParsed, TrackField.Artists);

                Predicate = UpdatePredicate(RestrictionType, _restrictionUnits);

                OnPropertyChanged("Artists");
            }
        }

        /// <summary>
        ///     Titles are seperated using comma
        /// </summary>
        public String Titles {
            get {return IntersperseRestrictions(TrackField.Titles);}
            set {
                var titlesParsed = ParseRestrictionUnits(value, TrackField.Titles);

                _restrictionUnits = AddRestrictionUnits(titlesParsed, TrackField.Titles);

                Predicate = UpdatePredicate(RestrictionType, _restrictionUnits);

                OnPropertyChanged("Titles");
            }
        }

        public RestrictionType RestrictionType {
            get {return _restrictionType;}
            private set {
                _restrictionType = value;
                Predicate = UpdatePredicate(RestrictionType, _restrictionUnits);
                OnPropertyChanged("RestrictionType");
            }
        }

        public DateTime StartTime {
            get {return _startTime;}
            set {
                _startTime = value;
                OnPropertyChanged("StartTime");
                OnPropertyChanged("IsActive");
            }
        }

        public DateTime EndTime {
            get {return _endTime;}
            set {
                _endTime = value;
                OnPropertyChanged("EndTime");
                OnPropertyChanged("IsActive");
            }
        }

        public bool IsActive {
            get {
                var now = DateTime.Now.TimeOfDay;
                var startTimeCompare = StartTime.TimeOfDay.CompareTo(now);
                var endTimeCompare = EndTime.TimeOfDay.CompareTo(now);
                return startTimeCompare <= 0 && endTimeCompare >= 0;
            }
        }

        /// <summary>
        ///     True means that it should be filtered.
        /// </summary>
        public Func<Track, bool> Predicate {
            get;
            private set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private IEnumerable<RestrictionUnit> AddRestrictionUnits(IEnumerable<RestrictionUnit> toAdd, TrackField field) {
            return _restrictionUnits.Where(unit => unit.Field != field)
                // get anything other than title restrictionunits
                                    .Concat(toAdd); // append the titles parsed to the existing restrictionunits
        }

        private IEnumerable<RestrictionUnit> ParseRestrictionUnits(String input, TrackField field) {
            var parsed = Enumerable.Empty<RestrictionUnit>();
            // only if the new value is not whitespace, try to split it up
            if(!string.IsNullOrWhiteSpace(input)) {
                // parse titles, and construct restrictionunits
                parsed = input.Split(',').Select(str => new RestrictionUnit(field, str.TrimStart()));
            }

            return parsed;
        }

        private string IntersperseRestrictions(TrackField field) {
            // find all restrictions of type field, and intersperse the elements with ','
            var ex1 = _restrictionUnits.Where(unit => unit.Field == field);
            return string.Join(", ", ex1.Select(unit => unit.FieldValue));
        }

        private Func<Track, bool> UpdatePredicate(RestrictionType restrictionType,
                                                  IEnumerable<RestrictionUnit> restrictionUnits) {
            var titles =
                restrictionUnits.Where(
                    unit => unit.Field == TrackField.Titles && !string.IsNullOrWhiteSpace(unit.FieldValue));
            var artists =
                restrictionUnits.Where(
                    unit => unit.Field == TrackField.Artists && !string.IsNullOrWhiteSpace(unit.FieldValue));

            Func<Track, bool> chainedFilter = t => false;

            if(!titles.Any() && !artists.Any())
                return chainedFilter;

            Func<RestrictionUnit, Func<Track, bool>> titleFilter = unit => {
                return (track => track.Name.ToLower().Contains(unit.FieldValue.Trim().ToLower()));
            };

            var titlePred = CombineOrTrackField(titles, titleFilter);

            Func<RestrictionUnit, Func<Track, bool>> artistFilter = unit => {
                return
                    (track => track.Album.Artists.Any(a => a.Name.ToLower().Contains(unit.FieldValue.Trim().ToLower())));
            };

            var artistPred = CombineOrTrackField(artists, artistFilter);

            chainedFilter = t => titlePred(t) && artistPred(t);

            if(restrictionType == RestrictionType.WhiteList) {
                // if whitelist, it means that if the predicate is false, it should not be filtered, so we negate
                var filter1 = chainedFilter;
                chainedFilter = t => !filter1(t);
            }

            return chainedFilter;
        }

        private Func<Track, bool> CombineOrTrackField(IEnumerable<RestrictionUnit> restrictionUnits,
                                                      Func<RestrictionUnit, Func<Track, bool>> filterFunc) {
            // if there are no restrictionUnits, the filter should just apply for everything
            if(!restrictionUnits.Any())
                return t => true;

            var predicates = restrictionUnits.Select(filterFunc); // map each unit to a filterFunction

            Func<Track, bool> predicate = t => {
                return predicates.Aggregate(false, (prevRes, pred) => prevRes || pred(t));
                // go through all predicates and OR them together
            };

            return predicate;
        }

        //ToStringPredicate

        private void OnPropertyChanged(string pName) {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(pName));
        }
    }
}