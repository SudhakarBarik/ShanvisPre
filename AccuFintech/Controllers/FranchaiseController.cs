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
    public class FranchaiseController : Controller
    {
        // GET: Franchaise
        FranchaiseModel franchaise = null;
        FranchaiseDal repo = null;
        GlobalFunction global = null;
        public FranchaiseController()
        {
            franchaise = new FranchaiseModel();
            repo = new FranchaiseDal();
            global = new GlobalFunction();
        }
        // GET: Master/FranchaiseMaster
        public ActionResult Index()
        {
            ViewBag.StateList = global.GetAllState();
            return View();
        }
        [HttpPost]
        public ActionResult Index(FranchaiseModel franchaisemaster)
        {
            if (ModelState.IsValid)
            {

                if (franchaisemaster.OpSection == "Insert")
                {
                    if (repo.AddNewFranchaise(franchaisemaster))
                    {
                        TempData["Status"] = "Ok";
                        TempData["Message"] = "Franchaise Added successfully.";
                    }
                    else
                    {
                        TempData["Status"] = "Failed";
                        TempData["Message"] = "Unable to Add Franchaise.";
                    }
                }
                else
                {
                    if (repo.UpdateFranchaise(franchaisemaster))
                    {
                        TempData["Status"] = "Ok";
                        TempData["Message"] = "Franchaise Updated successfully.";
                    }
                    else
                    {
                        TempData["Status"] = "Failed";
                        TempData["Message"] = "Unable to Update Franchaise.";
                    }
                }
                ModelState.Clear();
                return RedirectToAction("Index");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ViewBag.StateList = global.GetAllState();
                ModelState.AddModelError(string.Empty, "There is something wrong with Your Entry");
                return View();
            }
        }
        public JsonResult GetFranchaiseList()
        {
            var FranchaiseList = repo.GetAllFranchaisees();
            return Json(new { data = FranchaiseList }, JsonRequestBehavior.AllowGet);
        }

        public bool CheckFranchaise(string FranchaiseCode)
        {
            var brachList = repo.GetAllFranchaisees();
            System.Threading.Thread.Sleep(200);
            var SeachData = (from x in brachList where x.FranchaiseCode.ToLower() == FranchaiseCode.ToLower() select new { FranchaiseCode }).FirstOrDefault();
            if (SeachData != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public JsonResult GetFranchaisebyID(long Code)
        {
            var result = repo.FranchaisebyBId(Code);
            var bankDetails = JsonConvert.SerializeObject(result);
            return Json(bankDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteFranchaise(long brId)
        {
            if (repo.DeleteFranchaise(brId))
            {
                return Json("Franchaise Deleted successfully.", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Unable to Delete Franchaise.", JsonRequestBehavior.AllowGet);
            }
        }


    }
}