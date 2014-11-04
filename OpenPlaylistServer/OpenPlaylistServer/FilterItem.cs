using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlaylistServer {
    public class FilterItem {
        Object Attribute = new Object();
        Object Limit = new Object();
        Boolean White = new Boolean();
        public FilterItem(Object attribute, Object limit, Boolean white) {
            Attribute = attribute;
            Limit = limit;
            White = white;
        }
    }
}
