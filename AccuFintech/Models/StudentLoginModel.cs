using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class StudentLoginModel
    {
        public string StudentID { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
    }
}