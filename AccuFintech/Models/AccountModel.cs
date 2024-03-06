using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class AccountModel
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public string Password { get; set; }
        [Display(Name = "User Name")]
        public string Username { get; set; }
        public string UserType { get; set; }
    }
}