using AccuFintech.DataAccess;
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class PublishExamController : Controller
    {
        public PublishExamDal PEdal = null;
        public PublishExamModel PEModel = null;
        public PublishExamController()
        {
            PEdal = new PublishExamDal();
            PEModel = new PublishExamModel();
        }
        // GET: PublishExam
        public ActionResult Index()
        {
            ViewBag.QuestionsetList = new List<string>() { "Set-A", "Set-B", "Set-C", "Set-D", "Set-E" };
            PEModel.ConfigLists = PEdal.GetConfigurationList();
            return View(PEModel);
        }
        [HttpPost]
        public ActionResult Index(PublishExamModel PEModel)
        {
            bool Result = PEdal.SetExamdate(PEModel);
            if (Result == true)
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "Exam date Published Successfully";
            }
            else
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "Failed to save Exam Date !!";
            }
            return RedirectToAction("Index");
        }

        public ActionResult ResetExam(PublishExamModel PEModel)
        {
            Dictionary<string, string> Result = PEdal.ResetExam(PEModel.StudentID);
            if (Result["Status"] == "0")
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "Exam reset successfully done for studentID = " + PEModel.StudentID;
            }
            else
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "Failed to Reset Exam !!";
            }
            return RedirectToAction("Index");
        }
    }
}