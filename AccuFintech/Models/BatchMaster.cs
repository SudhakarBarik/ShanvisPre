using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class BatchMaster
    {
        [Required]
        public string CourseID { get; set; }
        [Required]
        public string Session { get; set; }
        [Required]
        public string BatchID { get; set; }
        [Required]
        public string BatchName { get; set; }
        [Required]
        public string BStartDate { get; set; }
        [Required]
        public string BEndDate { get; set; }
        [Required]
        public int CountTimes { get; set; }
        [Required]
        public string Mode { get; set; }
        [Required]
        public string Frequncy { get; set; }
        public string hiddenCheckedData { get; set; }
        public List<checkedDatesAndDays> CheckDtday { get; set; }
    }

    public class _Session
    {
        public int Id { get; set; }
        [Required]
        public string FDate { get; set; }
        [Required]
        public string TDate { get; set; }
        [Required]
        public string Name { get; set; }
        public string CreateDate { get; set; }
    }

    public class checkedDatesAndDays
    {
        public string date { get; set; }
        public string day { get; set; }
    }


}