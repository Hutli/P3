using System;

namespace OpenPlaylistServer {
    public class FilterItem {
        Object _attribute = new Object();
        Object _limit = new Object();
        Boolean _white;
        public FilterItem(Object attribute, Object limit, Boolean white) {
            _attribute = attribute;
            _limit = limit;
            _white = white;
        }
    }
}
