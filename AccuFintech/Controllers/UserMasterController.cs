using AccuFintech.DataAccess;
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class UserMasterController : Controller
    {
        public GlobalFunction global = null;
        public UserMasterDal UDal = null;
        public UserMasterController()
        {
            global = new GlobalFunction();
            UDal = new UserMasterDal();
        }
        // GET: UserMaster
        public ActionResult Index()
        {
            ViewBag.UserTypeList = global.LoadUserType();
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserMasterModel UM)
        {
            if (ModelState.IsValid)
            {
                if (UM.opsection == "INSERT")
                {
                    bool Result = UDal.UserEntry(UM);
                    if (Result == true)
                    {
                        TempData["Status"] = "Ok";
                        TempData["Message"] = "User master details added successfully";
                    }
                    else
                    {
                        TempData["Status"] = "Failed";
                        TempData["Message"] = "Failed to add User Master !!";
                    }
                }
                else
                {
                    bool Result = UDal.UpdateUserEntry(UM);
                    if (Result == true)
                    {
                        TempData["Status"] = "Ok";
                        TempData["Message"] = "User master details Updated successfully";
                    }
                    else
                    {
                        TempData["Status"] = "Failed";
                        TempData["Message"] = "Failed to Update User Master !!";
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = "Failed to Save User Master !!";
                return RedirectToAction("Index");
            }
        }

        public JsonResult GetAllUserJson()
        {
            List<UserMasterModel> Resultset = UDal.GetAllUser();
            return Json(Resultset, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUserByUserID(string UserID)
        {
            UserMasterModel Result = UDal.GetAllUserByID(UserID);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteUser(string UserID)
        {
            bool Result = UDal.DeleteUser(UserID);
            if (Result == true)
            {
                TempData["Status"] = "Ok";
                TempData["Message"] = "User master Deleted successfully";
            }
            else
            {
                TempData["Status"] = "Failed";
                TempData["Message"] = "Failed to delete User Master !!";
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
    }
}