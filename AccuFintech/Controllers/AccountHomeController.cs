using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuFintech.Controllers
{
    public class AccountHomeController : Controller
    {
        // GET: AccountHome
        public ActionResult Index()
        {
            return View();
        }
    }
}