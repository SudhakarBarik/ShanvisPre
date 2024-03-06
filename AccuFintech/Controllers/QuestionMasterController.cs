using AccuFintech.DataAccess;
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class QuestionMasterController : Controller
    {
        // GET: QuestionMaster
        public QuestionMasterDal QMdal = null;
        public QuestionMasterController()
        {
            QMdal = new QuestionMasterDal();
        }
        // GET: QuestionMaster
        public ActionResult Index()
        {
            ViewBag.QuestionsetList = new List<string>() { "Set-A", "Set-B", "Set-C", "Set-D", "Set-E" };
            return View();
        }
        [HttpPost]
        public ActionResult Index(QuestionMasterModel QModel)
        {
            if (ModelState.IsValid)
            {
                Dictionary<string, string> Result = QMdal.AddQuestions(QModel);
                if (Result["Status"].ToString() == "0")
                {
                    TempData["Status"] = "Ok";
                    TempData["Message"] = "Question Saved Successfully";
                }
                else
                {
                    TempData["Status"] = "Failed";
                    TempData["Message"] = "Failed to Save Question!!";
                }
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = "There is something wrong with Your Entry";
                return RedirectToAction("Index");
            }
        }

        public JsonResult GetquestionLists(string Qset)
        {
            List<QuestionList> Resultset = QMdal.GetQuestionList(Qset);
            return Json(Resultset, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQuestion(string QID)
        {
            QuestionMasterModel Result = QMdal.GetQuestionDetail(QID);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RemoveQuestion(string QID)
        {
            bool Result = QMdal.RemoveQuestion(QID);
            if (Result == true)
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "Question Deleted Successfully";
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = "Failed to delete this question !!";
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
    }
}