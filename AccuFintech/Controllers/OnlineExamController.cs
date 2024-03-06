using AccuFintech.DataAccess;
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class OnlineExamController : Controller
    {
        // GET: OnlineExam
        public OnlineExamDal OEDal = null;
        public OnlineExamModel OEModel = null;
        public OnlineExamController()
        {
            OEDal = new OnlineExamDal();
            OEModel = new OnlineExamModel();
        }
        // GET: OnlineExam
        public ActionResult Index()
        {
            ViewBag.title = "Online Exam";
            OEModel.DiscountLists = OEDal.GetAllDiscounts();
            return View(OEModel);
        }
        [HttpPost]
        public ActionResult Index(OnlineExamModel OEModel)
        {
            bool Result = OEDal.AddExamDetails(OEModel);
            TempData["Status"] = "Ok";
            TempData["Message"] = "Exam Completed !! Thank you.";
            return RedirectToAction("Index");
        }

        public JsonResult GetStudentdetails()
        {
            StudentDetails Result = OEDal.GetStudentDetails();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllQuestionLists(string Qset)
        {
            List<QuestionLists> Resultset = OEDal.GetAllQuestionByQset(Qset);
            return Json(Resultset, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQuestionByQid(string Qid, string QSet)
        {
            QuestionLists Result = OEDal.GetQuestionByQid(QSet, Qid);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult StuAnsweredQtn(QAnsModel QAModel)
        {
            bool Result = OEDal.AnsweredQuestion(QAModel);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExamdateValidate(string Questionset, string Examdate)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            //Result = OEDal.checkExamdate();
            Result.Add("Status", "0");
            if (Result["Status"].ToString() == "0")
            {
                Result = OEDal.CheckAlreadyExamGiven(Questionset, Examdate);
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAnsweredQuestionDetails(string Qset, string StudentID)
        {
            AnsModel Result = OEDal.GetAnswerDetails(Qset, StudentID);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExamTime()
        {
            int nowtime = OEDal.GetExamTime();
            return Json(nowtime, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ReviewMarked(string QSet, string QID)
        {
            bool Result = OEDal.MarkQtnAsReview(QSet, QID);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
    }
}