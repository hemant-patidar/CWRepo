﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommonWeal.NGOWeb.ViewModel
{
    public class DonateItem
    {
        public string Item{get; set; }
        public int ItemCount { get; set; }
        public int DonateCount { get; set; }
        public string Discription { get; set; }
        public int ItemID { get;set; }
        public int donatebyyou { get; set; }
        public int ItemRequire { get; set; }
        public int? DonatedByyou { get; set; }
    }
}