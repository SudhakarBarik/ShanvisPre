using AccuFintech.DataAccess;
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class UserTypeMasterController : Controller
    {
        public UserTypeMaster user = null;
        public UserTypeDal UTdal = null;
        public UserTypeMasterController()
        {
            user = new UserTypeMaster();
            UTdal = new UserTypeDal();
        }
        // GET: UserTypeMaster
        public ActionResult Index()
        {
            List<UserTypeMaster> utm = UTdal.GetAllUserType();
            return View(utm);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserTypeMaster usr)
        {
            if (ModelState.IsValid)
            {
                Boolean x = UTdal.InsertUserType(usr, "INSERT");
                if (x)
                {
                    ModelState.Clear();
                    TempData["Status"] = "Ok";
                    TempData["Message"] = "User Type Created Successfully.";
                }
                else
                {
                    TempData["Status"] = "Failed";
                    TempData["Message"] = "Unable to create User Type";
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            user = UTdal.SelectUserType(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(UserTypeMaster usr)
        {
            if (ModelState.IsValid)
            {
                Boolean x = UTdal.InsertUserType(usr, "UPDATE");
                if (x)
                {
                    ModelState.Clear();
                    TempData["Status"] = "Ok";
                    TempData["Message"] = "User Type Update Successfully.";
                }
                else
                {
                    TempData["Status"] = "Failed";
                    TempData["Message"] = "Unable to Upadte User Type";
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(usr);
            }
        }
    }
}