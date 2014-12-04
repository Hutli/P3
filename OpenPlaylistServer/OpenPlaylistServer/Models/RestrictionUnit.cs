using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenPlaylistServer.Models
{
    public class RestrictionUnit
    {
        public TrackField Field { get; private set; }
        public string FieldValue { get; private set; }
        public RestrictionType Type { get; private set; }

        public RestrictionUnit(TrackField field, String fieldValue, RestrictionType type)
        {
            Field = field;
            FieldValue = fieldValue;
            Type = type;
        }
    }
}
