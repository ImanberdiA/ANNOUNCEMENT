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
    #region Класс управления аутентификацией/авторизацией пользователя

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
                User user = await Manager.FindAsync(model.Name, model.Password);

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
        [Authorize]
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
                User user = new User { UserName = model.Name, Email = model.Email, PhoneNumber = model.PhoneNumber };

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

        #region Личный кабинет пользователя
        [Authorize]
        public async Task<ActionResult> AccountRoom()
        {
            User user = await Manager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (user != null)
            {
                string roleNames = null;
                foreach (var role in user.Roles)
                {
                    roleNames += string.Join("; ", RoleManager.Roles.Where(r => r.Id == role.RoleId).Select(r => r.Name));
                    roleNames += "; ";
                }

                ViewBag.RoleNames = roleNames;
                //ViewData["IsAuth"] = HttpContext.User.Identity.IsAuthenticated;
                return View(user);
            }
            else
            {
                return View("Error", new string[] { "Пользователь не существует" });
            }
        }
        #endregion

        private void AddErrorsToState(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }

        private AppUserManager Manager
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

    #endregion
}