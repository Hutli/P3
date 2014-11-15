using System;
using System.Collections.Generic;

namespace OpenPlaylistServer {
    public class Filter {
        private List<FilterItem> _filter = new List<FilterItem>();
        public void AddFilter(FilterItem filter) {
            _filter.Add(filter);
        }
        public void AddFilter(Object attribtute, Object limit, Boolean white) {
            FilterItem tmp = new FilterItem(attribtute, limit, white);
            _filter.Add(tmp);
        }

        public void ApplyFilter(List<PlaylistTrack> tracks){
            
        }
    }
}
