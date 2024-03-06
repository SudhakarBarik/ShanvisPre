using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class UserTypeMaster
    {
        [Required]
        [Display(Name = "User Type")]
        public string UserType { get; set; }
        [Required]
        [Display(Name = "Designation")]
        public string Designation { get; set; }
        public Boolean IsBackDate { get; set; }
        public Boolean IsUpdate { get; set; }

        public Boolean IsDelete { get; set; }
    }
}