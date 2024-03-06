using AccuFintech.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AccuFintech.Models;

namespace AccuFintech.Controllers
{
    public class AccountController : Controller
    {
        public AccountDal ACDal = null;
        public AccountController()
        {
            ACDal = new AccountDal();
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            @ViewBag.title = "Accufintech";
            return View();
        }
        [HttpPost]
        public ActionResult Login(AccountModel AM)
        {
            AccountModel loginuser = ACDal.Login(AM.UserID, AM.Password);

            if (loginuser.UserType != null)
            {
                if (loginuser.UserType.ToUpper() == "ADMINISTRATOR")
                {
                    FormsAuthentication.SetAuthCookie(loginuser.UserID, true);
                    Session["_UserId"] = loginuser.UserID;
                    Session["_UserType"] = loginuser.UserType.ToLower();
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(loginuser.UserID, true);
                    Session["_UserId"] = loginuser.UserID;
                    Session["_UserType"] = loginuser.UserType.ToLower();
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid User name and password");
                return View();
            }
        }
        public ActionResult Logout()
        {
            Session["_UserId"] = string.Empty;
            Session["_UserType"] = string.Empty;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", new { area = "" });
        }
    }
}