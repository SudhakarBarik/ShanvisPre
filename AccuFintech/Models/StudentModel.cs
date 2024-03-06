using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class StudentModel
    {
        [Required]
        public string CourseID { get; set; }
        public string StudentID { get; set; }
        [Required]
        public string Studentname { get; set; }
        [Required]
        public string Gurdian { get; set; }
        [Required]
        public string Age { get; set; }
        public string Email { get; set; }
        public string Husband { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string DOB { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string District { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Pincode { get; set; }
        [Required]
        public string Course { get; set; }

        public string CFees { get; set; }
        [Required]
        public string Batch { get; set; }
        [Required]
        public string Aadharno { get; set; }
        [Required]
        public string AcademicQualification { get; set; }
        public string StudentPicture { get; set; }
        public string StudentIDPicture { get; set; }
        public string StudentSignPicture { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Joinindate { get; set; }
        [Required]
        public string MartialStatus { get; set; }
        [Required]
        public string Franchaise { get; set; }
        public string AltPhoneno { get; set; }
        [Required]
        public string Religion { get; set; }
        [Required]
        public string Nationality { get; set; }
        [Required]
        public string Medium { get; set; }
        [Required]
        public string IsPhysicallyChallanged { get; set; }
        public string BStartDate { get; set; }
        public string BEndDate { get; set; }
        public string RegAmt { get; set; }
        public string PayType { get; set; }
        public string CourseCode { get; set; }
        public string CourseEnddate { get; set; }
        public HttpPostedFileBase StuPicture { get; set; }
        public HttpPostedFileBase StuIDPicture { get; set; }
        public HttpPostedFileBase StuSignPicture { get; set; }
        public string opsection { get; set; }
    }

    public class StudentList
    {
        public string StudentID { get; set; }
        public string Studentname { get; set; }
        public string Gurdian { get; set; }
        public string Phone { get; set; }
        public string Course { get; set; }
        public string Examdate { get; set; }
        public string Franchaise { get; set; }
        public string JoiningDate{ get; set; }
        public string ExamRegDate { get; set; }
    }

    public class StudentCourseModel
    {
        public string Coursecode { get; set; }
        public string Fees { get; set; }
        public string CourseEnddate { get; set; }
    }
}