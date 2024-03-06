using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class MarksSheetPrintModel 
    {
        public string StudentID { get; set; }
        public string CourseID { get; set; }
    }

    public class MarksheetDetails
    {
        public string StudentID { get; set; }
        public string Studentname { get; set; }
        public string Gurdian { get; set; }
        public string DOB { get; set; }
        public string Theory { get; set; }
        public string Practical { get; set; }
        public string MarksObtained { get; set; }
        public string Subject { get; set; }
        public string Coursename { get; set; }
        public string FranchaiseID { get; set; }
        public string Center { get; set; }
        public string Address { get; set; }
        public string FullMarks { get; set; }
        public string Passmarks { get; set; }
        public string TotalMarksobtained { get; set; }
        public string PercentageMarks { get; set; }
        public string Qrcode64 { get; set; }
    }
}