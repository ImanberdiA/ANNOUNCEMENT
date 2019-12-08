using Announcements.Models;
using Announcements.Settings;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Announcements.Controllers
{
    #region Управление пользователями
    [Authorize]
    public class UserAdminController : Controller
    {
        public ActionResult Index()
        {
            return View(Manager.Users);
        }

        #region Создание пользователя
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreatingUserModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.Name, Email = model.Email, PhoneNumber = model.PhoneNumber };

                IdentityResult result = await Manager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsToState(result);
                }
            }

            return View(model);
        }
        #endregion

        #region Удаление пользователя
        public async Task<ActionResult> Delete([Required]string id)
        {
            User user = await Manager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await Manager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] { "Пользователь не найден" });
            }
        }
        #endregion

        #region Редактирование пользователя
        public async Task<ActionResult> Edit([Required]string id)
        {
            if (ModelState.IsValid)
            {
                User user = await Manager.FindByIdAsync(id);

                if (user != null)
                {
                    return View(user);
                }
                else
                {
                    return View("Error", new string[] { "Пользователь не найден" });
                }
            }

            return View("Error", new string[] { "Пользователь не найден" });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, string email, string password)
        {
            User user = await Manager.FindByIdAsync(id);

            if (user != null)
            {
                user.Email = email;
                IdentityResult validUser = await Manager.UserValidator.ValidateAsync(user);
                if (!validUser.Succeeded)
                {
                    AddErrorsToState(validUser);
                }

                IdentityResult validPass = null;
                if (password != string.Empty)
                {
                    validPass = await Manager.PasswordValidator.ValidateAsync(password);

                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = Manager.PasswordHasher.HashPassword(password);
                    }
                    else
                    {
                        AddErrorsToState(validPass);
                    }
                }

                if ((validUser.Succeeded && validPass == null) || (validUser.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await Manager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsToState(result);
                    }
                }

                return View(user);
            }

            return View("Error", new string[] { "Пользователь не найден" });
        }
        #endregion

        private void AddErrorsToState(IdentityResult result)
        {
            foreach (var error in result.Errors)
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
    #endregion
}