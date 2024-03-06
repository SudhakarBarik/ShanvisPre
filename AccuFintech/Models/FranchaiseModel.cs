using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class FranchaiseModel
    {
        public long FranchaiseId { get; set; }

        [Display(Name = "Franchaise Code")]
        [Required]
        public string FranchaiseCode { get; set; }
        [Display(Name = "Franchaise Name")]
        [Required]
        public string FranchaiseName { get; set; }
        [Display(Name = "Address")]
        [Required]
        public string FranchaiseAddress { get; set; }
        [Display(Name = "Prefix")]
        [Required]
        public string FranchaisePrefix { get; set; }
        [Display(Name = "Mobile No")]
        [Required]
        [MinLength(10, ErrorMessage = "Mobile No must be 10 digit")]
        [MaxLength(10, ErrorMessage = "Mobile No. cannot be more than 10 digit")]
        public string FranchaiseMobileNo { get; set; }
        [Display(Name = "Manager Name")]
        [Required]
        public string FranchaiseManager { get; set; }
        [Display(Name = "Opening Date")]
        [Required]
        public string FranchaiseOpeningDate { get; set; }

        [Display(Name = "State")]
        [Required]
        public string StateName { get; set; }
        [Display(Name = "City")]
        [Required]
        public string CityName { get; set; }
        public string OpSection { get; set; }
    }
}