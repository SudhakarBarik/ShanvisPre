using AccuFintech.DataAccess;
using AccuFintech.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class FeesStructureController : Controller
    {
        FeesStructureDal Feesdal = null;
        GlobalFunction global = null;
        FeesStructureModel Feesmodel = null;

        public FeesStructureController()
        {
            Feesdal = new FeesStructureDal();
            global = new GlobalFunction();
            Feesmodel = new FeesStructureModel();
        }


        // GET: FeesStructure
        public ActionResult Index()
        {
            ViewBag.Franchaise = global.GetAllFranchaiseList();
            ViewBag.Studentlist = global.GetStudentList();
            ViewBag.payModeList = new List<string>() { "Cash", "UPI" };

            return View();
        }

        public ActionResult FeesPay()
        {
            ViewBag.Franchaise = global.GetAllFranchaiseList();
            ViewBag.Studentlist = global.GetStudentList();
            ViewBag.payModeList = new List<string>() { "Cash", "UPI" };
            return View();
        }
        [HttpPost]
        public ActionResult FeesPay(FeesStructureModel Feesmd)
        {
            ViewBag.Franchaise = global.GetAllFranchaiseList();
            ViewBag.Studentlist = global.GetStudentList();
            ViewBag.payModeList = new List<string>() { "Cash", "UPI" };
            bool isinsert = Feesdal.PayRemainFees(Feesmd);
            if (isinsert)
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "Payment Saved Successfully";
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = "Failed!!";
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetFeesByStudentID(string StudentId)
        {
            var result = Feesdal.GetFeesDetailsByStid(StudentId);
            var StDetails = JsonConvert.SerializeObject(result);
            return Json(StDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFeesStrFilter(string FranchaiseCode ,  string StudentId)
        {
            List<FeesStructureModel> Result = Feesdal.GetFeesDetailsByFilter(StudentId, FranchaiseCode);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllFeesByStudentId(string StudentId)
        {
            List<FeesStructureModel> Result = Feesdal.GetAllFeesDetailsByStudent(StudentId);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
    }
}