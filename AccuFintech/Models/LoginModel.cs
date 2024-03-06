using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class LoginModel
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string LogInBCode { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string EmailId { get; set; }
        public string RecoverBy { get; set; }
        public string RecoverAns { get; set; }
        public Nullable<bool> LockMode { get; set; }
        public Nullable<bool> OnlineStatus { get; set; }
        public string LastIP { get; set; }
        public string LastLocation { get; set; }
        public string FinKey { get; set; }
        public string LatCode { get; set; }
        public string LongCode { get; set; }
        public string UCode { get; set; }
        public Nullable<int> CLDNos { get; set; }
        public string ISOTemplate { get; set; }
        public byte[] PICS { get; set; }
        public Nullable<int> MPIN { get; set; }
        public string FYearName { get; set; }
        public Nullable<int> FinStartDate { get; set; }
        public Nullable<int> FinEndDate { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyName { get; set; }
    }
}