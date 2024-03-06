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
    public class BatchMasterController : Controller
    {
        GlobalFunction global = null;
        BatchMasterDal Batchdal = null;
        public BatchMasterController()
        {
            global = new GlobalFunction();
            Batchdal = new BatchMasterDal();
        }

        // GET: BatchMaster
        public ActionResult Index()
        {
            ViewBag.CourseList = global.GetAllCourses();
            ViewBag.SessionList = global.GetAllSessions();
            ViewBag.ModeList = new List<string>() { ".Wkly", ".Dly" }; ;
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.CourseList = global.GetAllCourses();
            ViewBag.SessionList = global.GetSessions();
            ViewBag.ModeList = new List<string>() { ".Wkly", ".Dly" }; ;

            return View();
        }
        [HttpPost]
        public ActionResult Create(BatchMaster batch)
        {
            bool isinsert = Batchdal.AddBatchDetails(batch);
            if (isinsert == true)
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "Batch Details Saved Successfully";
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = "Failed to save Batch Details";
            }
            return RedirectToAction("Index");
        }
        public bool CheckBatchId(string Batchid)
        {
            var batchList = Batchdal.GetAllBatches();
            System.Threading.Thread.Sleep(200);
            var SeachData = (from x in batchList where x.BatchID.ToLower() == Batchid.ToLower() select new { Batchid }).FirstOrDefault();
            if (SeachData != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public JsonResult GetBatchDetailsById(string BatchID)
        {
            var Result = Batchdal.GetBatchDetailsByID(BatchID);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBatchDetailsBySession(string SessionID)
        {
            var Result = Batchdal.GetBatchDetailsBySessionID(SessionID);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RemoveBatchDetails(string BatchID)
        {
            Dictionary<string, string> Result = Batchdal.RemoveBatchDetails(BatchID);
            if (Result["status"].ToString() == "0")
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "Batch Details Removed Successfully";
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = "Failed to remove Batch details";
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }


    }
}