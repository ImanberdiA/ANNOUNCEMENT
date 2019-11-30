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
    public class UserAdminController : Controller
    {
        // GET: UserAdmin
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
                AppUser user = new AppUser { UserName = model.Name, Email = model.Email, PhoneNumber = model.PhoneNumber };

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
            AppUser user = await Manager.FindByIdAsync(id);

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
}