using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class STUICardModel
    {
        public string StudentID { get; set; }
    }

    public class IcardModel
    {
        public string StudentID { get; set; }
        public string Studentname { get; set; }
        public string StudentPicture { get; set; }
        public string StudentSignature { get; set; }
        public string DOB { get; set; }
        public string JoiningDate { get; set; }
        public string Examdate { get; set; }
        public string Course { get; set; }
        public string FranchiseAddress { get; set; }
    }
}