using Announcements.Models;
using Announcements.Settings;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
                    ClaimsIdentity ident = await Manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, ident);

                    if (returnUrl != string.Empty)
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", null);
                    }
                }
            }
            else
            {
                return View("Error", new string[] { "Произошла ошибка" });
            }

            return View(model);
        }
        #endregion

        #region Выход из системы
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Login", "Authentication");
        }

        #endregion

        #region Регистрация пользователя
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(CreatingUserModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Name, Email = model.Email, PhoneNumber = model.PhoneNumber };

                IdentityResult result = await Manager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await Login(new AuthUserModel { Name = model.Name, Password = model.Password }, "/Home/Index");
                }
                else
                {
                    AddErrorsToState(result);
                }
            }

            return View(model);
        }
        #endregion

        private void AddErrorsToState(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public AppUserManager Manager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        public IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

    }
}