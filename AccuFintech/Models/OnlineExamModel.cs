using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class OnlineExamModel
    {
        public string StudentID { get; set; }
        public string Studentname { get; set; }
        public string Startdatetime { get; set; }
        public string timer { get; set; }
        public string NoOfAttempts { get; set; }
        public string NoOfReview { get; set; }
        public string Pending { get; set; }
        public string QuestionSet { get; set; }
        public string AETotalNoOfQuestions { get; set; }
        public string AENoOfAttemptedQuestions { get; set; }
        public string AENoOfReviewedQuestions { get; set; }
        public string AENoOfPendingQuestions { get; set; }
        public string AETotalMarks { get; set; }
        public string StuExamdate { get; set; }
        public string EStartTime { get; set; }
        public string EEndtime { get; set; }
        public string TotalSeconds { get; set; }
        public List<DiscountList> DiscountLists { get; set; }
    }

    public class DiscountList
    {
        public string MarksFrom { get; set; }
        public string MarksTo { get; set; }
        public string FeesDiscount { get; set; }
    }

    public class StudentDetails
    {
        public string StudentID { get; set; }
        public string Studentname { get; set; }
        public string QSet { get; set; }
        public string StuExamdate { get; set; }
    }

    public class QuestionLists
    {
        public string QuestionID { get; set; }
        public string Question { get; set; }
        public string Ans1 { get; set; }
        public string Ans2 { get; set; }
        public string Ans3 { get; set; }
        public string Ans4 { get; set; }
        public string Answered { get; set; }
        public string AnsOption { get; set; }
        public string RightAns { get; set; }
        public string IsCompleted { get; set; }
    }

    public class QAnsModel
    {
        public string StudentID { get; set; }
        public string QuestionID { get; set; }
        public string QuestionSet { get; set; }
        public string AnsOption { get; set; }
    }

    public class AnsModel
    {
        public int TotalNoOfQuestions { get; set; }
        public int NoOfPendingQuestion { get; set; }
        public int NoOfQuestionAnswered { get; set; }
        public int NoOfReviewedQuestion { get; set; }
        public int TotalMarks { get; set; }
    }
}