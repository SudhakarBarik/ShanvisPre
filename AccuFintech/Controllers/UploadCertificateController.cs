using AccuFintech.DataAccess;
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class UploadCertificateController : Controller
    {
        public GlobalFunction global = null;
        public UploadCertificateDal UCDal = null;
        public UploadCertificateController()
        {
            global = new GlobalFunction();
            UCDal = new UploadCertificateDal();
        }
        // GET: UploadCertificate
        public ActionResult Index()
        {
            ViewBag.FranchaiseList = global.GetFranchaiseList();
            ViewBag.CourseList = global.GetCourses();
            ViewBag.StudentList = "";
            return View();
        }

        public JsonResult Getstudents(string Course, string Franchaise)
        {
            List<DropdownModel> Result = UCDal.GetStudentList(Course, Franchaise);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UploadDocuments(UploadCertificateModel UM)
        {
            Dictionary<string, string> Result = UCDal.UploadDocuments(UM);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetUploadedDocuments(string Student, string Course, string Franchaise)
        {
            Dictionary<string, string> Result = UCDal.GetUploadedDocuments(Student, Course, Franchaise);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
    }
}
