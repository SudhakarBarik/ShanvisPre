using AccuFintech.DataAccess;
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class SubjectMasterController : Controller
    {
        public GlobalFunction global = null;
        public SubjectMasterDal SMDal = null;
        
        public SubjectMasterController()
        {
            global = new GlobalFunction();
            SMDal = new SubjectMasterDal();
        }
        // GET: SubjectMaster
        public ActionResult Index()
        {
            ViewBag.CourseList = global.GetAllCourses();
            return View();
        }
        [HttpPost]
        public ActionResult Index(SubjectModel SM)
        {
            Dictionary<string, string> Result = SMDal.AddOrUpdateSubject(SM);
            if (Result["status"].ToString() == "0")
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "Subject Details Saved Successfully";
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = "Failed to save Subject Details";
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetAllSubjectByCourse(string CourseID)
        {
            List<SubjectModel> Result = SMDal.GetSubjectList(CourseID);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
    }
}