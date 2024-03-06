using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class FeesStructureModel
    {
        public int id { get; set; }
        public string Studentid { get; set; }
        public string FranchaiseCode { get; set; }
        public string Course { get; set; }
        public string Batch { get; set; }
        public string TotalFees { get; set; }
        [Required]
        public string PaidFees { get; set; }
        public string TotalPaid { get; set; }
        public string  RemainFee { get; set; }
        public string PayDate { get; set; }
        [Required]
        public string PayType { get; set; }

    }
}