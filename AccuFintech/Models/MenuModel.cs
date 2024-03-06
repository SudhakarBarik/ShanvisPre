using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class MenuModel
    {
        public long MenuID { get; set; }
        public string MenuName { get; set; }
        public long MenuParentID { get; set; }
        public string ControllerName { get; set; }
        public string AreaName { get; set; }
        public string ActionName { get; set; }
        public string Icon { get; set; }
    }
}