using Announcements.Models;
using Announcements.Settings;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Announcements.Controllers
{
    public class AuthenticationController : Controller
    {
        #region Вход в систему
        public ActionResult Login(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AuthUserModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await Manager.FindAsync(model.Name, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Некорректное имя или пароль");
                }
                else
                {
                    //ClaimsIdentity identity = await Manager.
                }
            }
            else
            {
                return View("Error", new string[] { "Произошла ошибка" });
            }
        }
        #endregion

        public AppUserManager Manager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }



    }
}