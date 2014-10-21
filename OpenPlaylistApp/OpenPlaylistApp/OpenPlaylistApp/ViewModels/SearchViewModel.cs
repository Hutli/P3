﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp2
{
    public class SearchViewModel : BaseViewModel
    {
        private SearchResultView result;
        public SearchResultView Result { get { return result; } set { SetProperty(ref result, value, "Result"); } }
    }
}