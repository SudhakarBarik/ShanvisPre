using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class MarksEntryModel
    {
        public string StudentID { get; set; }
        public string StudentIDView { get; set; }
        public string Studentname { get; set; }
        public string Gurdianname { get; set; }
        public string DOJ { get; set; }
        public string ExamRegdate { get; set; }
        public string Examdate { get; set; }
        public string Franchaisecode { get; set; }
        public string Franchaise { get; set; }
        public string CourseID { get; set; }
        public string Course { get; set; }
        public string StudentMarksJson { get; set; }
    }

    public class SubjectMarksModel
    {
        public string SubjectID { get; set; }
        public string Subject { get; set; }
        public string FullMarks { get; set; }
        public string PassMarks { get; set; }
        public string Theory { get; set; }
        public string Practical { get; set; }
        public string MarksObtained { get; set; }
        public string StudentID { get; set; }
        public string CourseID { get; set; }
    }
}