using AccuFintech.DataAccess;
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class ExamReportController : Controller
    {
        public ExamReportDal ERDal = null;
        public GlobalFunction global = null;
        public ExamReportController()
        {
            ERDal = new ExamReportDal();
            global = new GlobalFunction();
        }
        // GET: ExamReport
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetExamReportList(string Fromdate, string Todate)
        {
            List<ExamReportListModel> Result = ERDal.GetExamReportList(Fromdate, Todate);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
    }
}