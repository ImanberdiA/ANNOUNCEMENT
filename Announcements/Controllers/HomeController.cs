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
            //return View(db.Announs.ToList());
            return View();
        }

        [HttpPost]
        public ActionResult AnnSearch(string name)
        {
            var allAnns = db.Announs.Where(a => a.Description.Contains(name)).ToList();
            if (allAnns.Count <= 0)
            {
                return HttpNotFound();
            }
            return PartialView(allAnns);
        }
    }
}