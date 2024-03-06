using AccuFintech.DataAccess;
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class MenuController : Controller
    {
        List<MenuModel> MenuList = null;
        public MenuDal MDal = null;
        public MenuController()
        {
            MenuList = new List<MenuModel>();
            MDal = new MenuDal();
        }
        // GET: Menu
        public ActionResult Index()
        {
            List<MenuModel> MenuList = new List<MenuModel>();

            if (!string.IsNullOrEmpty(Session["_UserType"].ToString().ToUpper()))
            {
                if (Session["_UserType"].ToString().ToUpper() == "ADMINISTRATOR")
                {
                    MenuList = MDal.get_Menu();
                }
                else
                {
                    MenuList = MDal.get_Menu(Session["_UserType"].ToString().ToUpper());
                }
            }

            return PartialView("~/Views/Menu/_LayoutMenu.cshtml", MenuList);
        }
    }
}