
using AccuFintech.DataAccess;
using AccuFintech.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class SessionYearController : Controller
    {
        GlobalFunction global = null;
        BatchMasterDal Batchdal = null;
        public SessionYearController()
        {
            global = new GlobalFunction();
            Batchdal = new BatchMasterDal();
        }
        // GET: SessionYear
        public ActionResult Index()
        {
            ViewBag.CourseList = global.GetAllCourses();
            ViewBag.SessionList = global.GetAllSessions();
            ViewBag.ModeList = new List<string>() { ".Wkly", ".Dly" }; ;
            return PartialView("_CreateSession");
        }

        [HttpPost]
        public ActionResult Index(_Session SModel)
        {
            List<string> IsOverLap = global.IsDateOverLap(SModel.FDate, SModel.TDate);
            if (IsOverLap == null || IsOverLap.Count == 0)
            {
                if (Batchdal.AddNewSession(SModel))
                {
                    TempData["Status"] = "Ok";
                    TempData["Message"] = "Session Added successfully.";
                }
                else
                {
                    TempData["Status"] = "Failed";
                    TempData["Message"] = "Unable to Add Session.";
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "There Already Have Season Continued");
                ViewBag.CourseList = global.GetAllCourses();
                ViewBag.SessionList = global.GetAllSessions();
                return PartialView("_CreateSession");
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetSessionList()
        {
            var Sessions = Batchdal.GetAllSessions();
            return Json(new { data = Sessions }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteSession(int Id)
        {
            if (Batchdal.DeleteSession(Id))
            {
                return Json("Session Deleted successfully.", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Unable to Delete Session.", JsonRequestBehavior.AllowGet);
            }
        }
    }
}