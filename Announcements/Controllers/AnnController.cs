using Announcements.Models;
using Announcements.Settings;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Announcements.Controllers
{
    public class AnnController : Controller
    {
        AppIdentityDbContext db = new AppIdentityDbContext();

        // GET: Ann
        public ActionResult Index()
        {
            return View();
        }

        #region Создание объявление
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Announ ann)
        {
            if (ModelState.IsValid)
            {
                User cur_user = await Manager.FindByNameAsync(HttpContext.User.Identity.Name);
                cur_user.Announs.Add(ann);
                await Manager.UpdateAsync(cur_user);
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }

            return View(ann);
        }
        #endregion

        private void AddErrorsToState(IdentityResult res)
        {
            foreach (string error in res.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        
        private AppUserManager Manager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}