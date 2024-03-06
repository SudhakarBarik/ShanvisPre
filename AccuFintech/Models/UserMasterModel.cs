using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class UserMasterModel
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserType { get; set; }
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Phone { get; set; }
        public bool LockMode { get; set; }
        public string LockModeStatus { get; set; }
        public string opsection { get; set; }
    }
}