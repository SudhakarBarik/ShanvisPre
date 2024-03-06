using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class DropdownModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class color
    {
        public string name { get; set; }
        public string hex { get; set; }
        public string rgb { get; set; }
        public string[] families { get; set; }
    }
}