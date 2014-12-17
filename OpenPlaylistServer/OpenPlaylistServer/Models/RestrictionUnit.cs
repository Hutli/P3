using System;

namespace OpenPlaylistServer.Models
{
    public class RestrictionUnit
    {
        public RestrictionUnit(TrackField field, String fieldValue)
        {
            Field = field;
            FieldValue = fieldValue;
        }

        public TrackField Field { get; private set; }
        public string FieldValue { get; private set; }
    }
}