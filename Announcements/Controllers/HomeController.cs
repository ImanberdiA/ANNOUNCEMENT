using Announcements.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Announcements.Controllers
{
    public class HomeController : Controller
    {
        AppIdentityDbContext db = new AppIdentityDbContext();

        public ActionResult Index()
        {
            return View(db.Announs.ToList());
        }
    }
}