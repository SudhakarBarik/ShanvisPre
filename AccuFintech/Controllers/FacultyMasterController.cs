using AccuFintech.DataAccess;
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class FacultyMasterController : Controller
    {
        GlobalFunction global = null;
        FacultyDal fdal= null;
        FacultyModel Famodel = null;
        public FacultyMasterController()
        {
            global = new GlobalFunction();
            fdal = new FacultyDal();
            Famodel = new FacultyModel();
        }
        // GET: FacultyMaster
        public ActionResult Index()
        {
            ViewBag.FranchaiseList = global.GetAllFranchaiseList();
            ViewBag.GenderList = new List<string>() { "Male", "Female" };

            return View();
        }

        public JsonResult GetFacultyFranchaiseWise(string FranchaiseCode)
        {
            List<FacultyModel> Result = fdal.GetFacultyFranchaiseWise(FranchaiseCode);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.FranchaiseList = global.GetFranchaiseList();
            ViewBag.GenderList = new List<string>() { "Male", "Female" };

            return View();
        }
        [HttpPost]
        public ActionResult Create(FacultyModel facmodel)
        {
            facmodel.opsection = "INSERT";
            bool isinsert = fdal.AddOrUpdateFaculty(facmodel);
            if (isinsert == true)
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "Faculty Details Saved Successfully";
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = "Failed to save Faculty Details";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string FacultyID)
        {
            ViewBag.FranchaiseList = global.GetFranchaiseList();
            ViewBag.GenderList = new List<string>() { "Male", "Female" };
            Famodel = fdal.GetfacultyByID(FacultyID);
            ViewBag.ProfilePicPath = Famodel.FacPicturePath;
            return View("Edit", Famodel);
        }
        [HttpPost]
        public ActionResult Edit(FacultyModel facmodel)
        {
            facmodel.opsection = "UPDATE";
            bool isinsert = fdal.AddOrUpdateFaculty(facmodel);
            if (isinsert == true)
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "Faculty Details Updated Successfully";
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = "Failed to Update Faculty Details";
            }
            return RedirectToAction("Index");
        }

        public JsonResult RemoveFaculty(string FacultyID)
        {
            Dictionary<string, string> Result = fdal.RemoveFaculty(FacultyID);
            if (Result["status"].ToString() == "0")
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "faculty Details Removed Successfully";
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = "Failed to remove faculty details";
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
    }
}