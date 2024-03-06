using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class CourseMasterModel
    {
        [Required]
        public string Programcode { get; set; }
        [Required]
        public string Coursename { get; set; }
        [Required]
        public string CourseModule { get; set; }
        [Required]
        public string MonthDuration { get; set; }
        [Required]
        public string HourDuration { get; set; }
        [Required]
        public string Eligibility { get; set; }
        public string  Fees{ get; set; }
        [Required]
        public string CareerOportunities { get; set; }
        public string opsection { get; set; }
        public List<CourseMasterModel> Courses { get; set; }
    }
}