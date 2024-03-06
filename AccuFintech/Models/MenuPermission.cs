using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Models
{
    public class MenuPermission
    {
        public string UserType { get; set; }
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public int MenuParentID { get; set; }
        public Boolean IsAsign { get; set; }
    }

    public class UserMenu
    {
        [Required]
        public string UserType { get; set; }
        public List<MenuPermission> menuPermission { get; set; }
        public SelectList UsertypeList { get; set; }
    }
}