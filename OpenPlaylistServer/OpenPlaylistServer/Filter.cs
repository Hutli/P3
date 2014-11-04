using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlaylistServer {
    public class Filter {
        private List<FilterItem> Filter = new List<FilterItem>();
        public void AddFilter(FilterItem filter) {
            Filter.Add(filter);
        }
        public void AddFilter(Object attribtute, Object limit, Boolean white) {
            FilterItem tmp = new FilterItem(attribtute, limit, white);
            Filter.Add(tmp);
        }

        public void ApplyFilter(List<PlaylistTrack> tracks){
            
        }
    }
}
