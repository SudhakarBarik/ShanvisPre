using AccuFintech.DataAccess;
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class StudentRegistrationController : Controller
    {
        public StudentRegistrationDal STDal = null;
        public GlobalFunction global = null;
        public StudentModel SM = null;
        public StudentRegistrationController()
        {
            STDal = new StudentRegistrationDal();
            global = new GlobalFunction();
            SM = new StudentModel();
        }
        // GET: StudentRegistration
        public ActionResult Index()
        {
            ViewBag.CourseList = global.GetCourses();
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.GenderList = new List<string>() { "Male", "Female" };
            ViewBag.AcademicQualifications = new List<string>() { "VII", "IX", "X", "XI", "XII", "Graduate", "Post Graduate" };
            ViewBag.CourseList = global.GetCourses();
            ViewBag.MartialStatusList = new List<string>() { "Married", "UnMarried" };
            ViewBag.FranchaiseList = global.GetFranchaiseList();
            ViewBag.MediumList = new List<string>() { "Odia", "Hindi", "English" };
            ViewBag.PhysicallychallangeList = new List<string>() { "Yes", "No" };
            ViewBag.payModeList = new List<string>() { "Cash","UPI" };
            return View();
        }

        [HttpPost]
        public ActionResult Create(StudentModel SM)
        {
            SM.opsection = "INSERT";
            bool Result = STDal.AddOrUpdateStudent(SM);
            if (Result == true)
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "Student Details Saved Successfully";
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = "Failed to save Student Details";
            }
            return RedirectToAction("Create");
        }

        public JsonResult GetStudentCourseWise(string CourseID)
        {
            List<StudentList> Result = STDal.GetStudentsCourseWise(CourseID);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBatchCourseWise(string CourseID)
        {
            var Result = STDal.GetBatchesCourseWise(CourseID);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCourseDetails(string CourseID, string CourseStartdate)
        {
            StudentCourseModel Result = STDal.GetCourseDetails(CourseID, CourseStartdate);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(string StudentID)
        {
            ViewBag.GenderList = new List<string>() { "Male", "Female" };
            ViewBag.AcademicQualifications = new List<string>() { "VII", "IX", "X", "XI", "XII", "Graduate", "Post Graduate" };
            ViewBag.CourseList = global.GetCourses();
            ViewBag.BatchList = global.GetAllBatches();
            ViewBag.MartialStatusList = new List<string>() { "Married", "UnMarried" };
            ViewBag.MediumList = new List<string>() { "Bengali", "Hindi", "English" };
            ViewBag.PhysicallychallangeList = new List<string>() { "Yes", "No" };
            SM = STDal.GetStudentDetails(StudentID);
            ViewBag.FranchaiseList = global.GetFranchaiseList();
            ViewBag.ProfilePicPath = SM.StudentPicture;
            ViewBag.IdProofPicPath = SM.StudentIDPicture;
            ViewBag.SignProofPath = SM.StudentSignPicture;
            return View(SM);
        }
        [HttpPost]
        public ActionResult Edit(StudentModel SM)
        {
            SM.opsection = "UPDATE";
            bool Result = STDal.AddOrUpdateStudent(SM);
            if (Result == true)
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "Student Details Saved Successfully";
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = "Failed to save Student Details";
            }
            return RedirectToAction("Index");
        }

        public JsonResult RemoveStudent(string StudentID)
        {
            Dictionary<string, string> Result = STDal.RemoveStudent(StudentID);
            if (Result["status"].ToString() == "0")
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "Student Details Removed Successfully";
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = "Failed to remove student details";
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
    }
}