using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class ExamReportModel
    {
        public string Fromdate { get; set; }
        public string Todate { get; set; }
    }

    public class ExamReportListModel
    {
        public string StudentID { get; set; }
        public string Studentname { get; set; }
        public string Questionset { get; set; }
        public string Starttime { get; set; }
        public string Endtime { get; set; }
        public string TotalQuestion { get; set; }
        public string NoOfAttempt { get; set; }
        public string NoOfPending { get; set; }
        public string Marks { get; set; }
        public string Discount { get; set; }
    }
}