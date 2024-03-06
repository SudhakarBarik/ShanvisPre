using AccuFintech.DataAccess;
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class CourseMasterController : Controller
    {
        public CourseMasterModel CMModel = null;
        public CourseMasterDal CMdal = null;
        public CourseMasterController()
        {
            CMdal = new CourseMasterDal();
            CMModel = new CourseMasterModel();
        }
        // GET: CourseMaster
        public ActionResult Index()
        {
            CMModel.Courses = CMdal.GetAllCourses();
            return View(CMModel);
        }
        [HttpPost]
        public ActionResult Index(CourseMasterModel CM)
        {
            Dictionary<string, string> Result = CMdal.AddOrUpdateCourse(CM);
            if (Result["status"].ToString() == "0")
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = Result["msg"];
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = Result["msg"];
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetCourseByID(string CourseID)
        {
            CourseMasterModel Result = CMdal.GetCourseByID(CourseID);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RemoveCourse(string Programcode)
        {
            Dictionary<string, string> Result = CMdal.RemoveCourse(Programcode);
            if (Result["status"] == "0")
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = Result["msg"];
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = Result["msg"];
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public bool CheckProgramCode(string pgcode)
        {
            var CourseList = CMdal.GetAllCourses();
            System.Threading.Thread.Sleep(200);
            var SeachData = (from x in CourseList where x.Programcode.ToLower() == pgcode.ToLower() select new { pgcode }).FirstOrDefault();
            if (SeachData != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}