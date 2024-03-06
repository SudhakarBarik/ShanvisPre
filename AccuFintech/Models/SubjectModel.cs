using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class SubjectModel
    {
        [Required]
        public string CourseID { get; set; }
        public string Course { get; set; }
        public string Subject { get; set; }
        public string FullMarks { get; set; }
        public string PassMarks { get; set; }
        [Required]
        public string SubjectString { get; set; }
    }
}