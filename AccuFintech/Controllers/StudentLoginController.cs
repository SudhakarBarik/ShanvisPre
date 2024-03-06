using AccuFintech.DataAccess;
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AccuFintech.Controllers
{
    public class StudentLoginController : Controller
    {
        // GET: StudentLogin
        public AccountDal ACDal = null;
        public StudentLoginController()
        {
            ACDal = new AccountDal();
        }

        // GET: StudentLogin
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
        public ActionResult Login(StudentLoginModel SL)
        {
            StudentLoginModel loginuser = ACDal.StudentLogin(SL.StudentID, SL.Password);
            if (loginuser.UserType != null)
            {
                if (loginuser.UserType.ToUpper() == "STUDENT")
                {
                    FormsAuthentication.SetAuthCookie(loginuser.StudentID, true);
                    Session["_UserId"] = loginuser.StudentID;
                    Session["_UserType"] = loginuser.UserType.ToLower();
                    return RedirectToAction("Index", "StudentDashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid StudentID and password");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid StudentID and password");
                return View();
            }
        }
    }
}