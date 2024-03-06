using AccuFintech.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class StudentICardPrintController : Controller
    {
        public MarksSheetPrintDal MSPDal = null;
        public GlobalFunction global = null;
        public StudentICardPrintController()
        {
            MSPDal = new MarksSheetPrintDal();
            global = new GlobalFunction();
        }
        // GET: StudentICardPrint
        public ActionResult Index()
        {
            ViewBag.StudentList = global.GetStudentList();
            return View();
        }
    }
}