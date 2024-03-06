using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class CertificateModel
    {
        public string StudentID { get; set; }
    }

    public class CertificatePrintDetails
    {
        public string StudentID { get; set; }
        public string Studentname { get; set; }
        public string Gurdian { get; set; }
        public string StudentPicture { get; set; }
        public string StartingMonth { get; set; }
        public string EndingMonth { get; set; }
        public string FranchaiseID { get; set; }
        public string Center { get; set; }
        public string Address { get; set; }
        public string DateOfIssue { get; set; }
        public string Coursename { get; set; }
    }
}
