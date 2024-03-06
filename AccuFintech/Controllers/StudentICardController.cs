using AccuFintech.DataAccess;
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class StudentICardController : Controller
    {
        public GlobalFunction global = null;
        public StudentModel SM = null;
        public StudentRegistrationDal SRDal = null;
        public StudentICardController()
        {
            global = new GlobalFunction();
            SM = new StudentModel();
            SRDal = new StudentRegistrationDal();
        }
        // GET: StudentICard
        public ActionResult Index()
        {
            ViewBag.StudentList = global.GetStudentList();
            return View();
        }

        public ActionResult GetStudentICard(string StudentID)
        {
            SM = SRDal.GetStudentDetails(StudentID);
            ViewBag.StudentPics = SM.StudentPicture;
            return PartialView("/Views/StudentICard/GetStudentICard.cshtml", SM);
        }
    }
}