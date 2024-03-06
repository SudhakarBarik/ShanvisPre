using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class UploadCertificateModel
    {
        public string Franchaise { get; set; }
        public string Student { get; set; }
        public string Course { get; set; }
        public string StrCertificate { get; set; }
        public string StrMarksheet { get; set; }
        public HttpPostedFileBase Certificate { get; set; }
        public HttpPostedFileBase Marksheet { get; set; }
    }

}