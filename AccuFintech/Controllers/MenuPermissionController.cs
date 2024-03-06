using AccuFintech.DataAccess;
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class MenuPermissionController : Controller
    {
        public MenuPermissionDal MPdal = null;
        public UserMenu um = null;
        public MenuPermissionController()
        {
            MPdal = new MenuPermissionDal();
            um = new UserMenu();
        }
        // GET: MenuPermission
        public ActionResult Index()
        {
            if (TempData["MenuModel"] == null)
            {
                List<MenuPermission> menulist = MPdal.LoadAllMenu("");
                um.UserType = "";
                um.menuPermission = menulist;
                ViewBag.UserTypeList = MPdal.LoadUserType().Where(n => n.Key != "Administrator").ToList();
                return View(um);
            }
            else
            {
                um = (UserMenu)TempData["MenuModel"];
                ViewBag.UserTypeList = MPdal.LoadUserType().Where(n => n.Key != "Administrator").ToList();
                return View(um);
            }
        }

        [HttpPost]
        public ActionResult Index(UserMenu mnu)
        {
            if (ModelState.IsValid)
            {
                Boolean x = MPdal.InsertMenus(mnu, mnu.UserType);
            }
            TempData["MenuModel"] = null;
            ViewBag.UserTypeList = MPdal.LoadUserType().Where(n => n.Key != "Administrator").ToList();
            return RedirectToAction("Index", new { area = "Admin" });
        }
        [HttpGet]
        public ActionResult LoadAsignedMenu(string userType)
        {
            List<MenuPermission> menulist = MPdal.LoadAllMenu(userType);
            um.UserType = userType;
            um.menuPermission = menulist;
            ViewBag.UserTypeList = MPdal.LoadUserType().Where(n => n.Key != "Administrator").ToList();
            TempData["MenuModel"] = um;
            return RedirectToAction("Index", new { area = "Admin" });
        }
    }
}