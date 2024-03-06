using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class FacultyModel
    {
        public string FacultyID { get; set; }
        [Required]
        public string FranchaiseCode { get; set; }
        [Required]
        public string FacultyName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [RegularExpression(@"^([0-9]{4}[0-9]{4}[0-9]{4}$)|([0-9]{4}\s[0-9]{4}\s[0-9]{4}$)|([0-9]{4}-[0-9]{4}-[0-9]{4}$)", ErrorMessage = "Invalid Aadhaar Number.")]
        public string AadharNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "PinCode must be numeric")]
        public string PinCode { get; set; }
        [Required]
        public string MaxQualification { get; set; }
        [Required]
        public string Department { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "TotalExp must be numeric")]
        public string TotalExp { get; set; }
        [Required]
        public string Password { get; set; }
        public string FacPicturePath { get; set; }
        public HttpPostedFileBase FacPicture { get; set; }
        public string opsection { get; set; }
    }
}