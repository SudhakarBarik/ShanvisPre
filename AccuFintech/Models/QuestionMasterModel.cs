using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class QuestionMasterModel
    {
        public string Qid { get; set; }
        [Required]
        public string QSet { get; set; }
        [Required]
        public string Question { get; set; }
        [Required]
        public string Ans1 { get; set; }
        [Required]
        public string Ans2 { get; set; }
        [Required]
        public string Ans3 { get; set; }
        [Required]
        public string Ans4 { get; set; }
        [Required]
        public string RightAns { get; set; }
        public string opsection { get; set; }
    }

    public class QuestionList
    {
        public string QID { get; set; }
        public string Question { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string RightAnswer { get; set; }
    }
}