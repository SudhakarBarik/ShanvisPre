using AccuFintech.DataAccess;
using AccuFintech.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class StudentProfileController : Controller
    {
        public StudentModel SM = null;
        public StudentRegistrationDal SRDal = null;
        public GlobalFunction global = null;
        public StudentProfileController()
        {
            SM = new StudentModel();
            SRDal = new StudentRegistrationDal();
            global = new GlobalFunction();
        }
        // GET: StudentProfile
        public ActionResult Index()
        {
            ViewBag.GenderList = new List<string>() { "Male", "Female" };
            ViewBag.AcademicQualifications = new List<string>() { "VII", "IX", "X", "XI", "XII", "Graduate", "Post Graduate" };
            ViewBag.CourseList = global.GetCourses();
            ViewBag.MartialStatusList = new List<string>() { "Married", "UnMarried" };
            ViewBag.FranchaiseList = global.GetFranchaiseList();
            string StudentID = Session["_UserId"].ToString();
            SM = SRDal.GetStudentDetails(StudentID);
            ViewBag.ProfilePicPath = SM.StudentPicture;
            ViewBag.IdProofPicPath = SM.StudentIDPicture;
            return View(SM);
        }
    }
}