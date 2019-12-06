using Announcements.Models;
using Announcements.Settings;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Announcements.Controllers
{
    public class RoleAdminController : Controller
    {
        // GET: RoleAdmin
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        #region Создание роли
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([Required]string name)
        {
            if (ModelState.IsValid)
            {
                Role role = new Role(name);
                IdentityResult res = await RoleManager.CreateAsync(role);

                if (res.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsToState(res);
                }
            }

            return View(name);
        }
        #endregion

        #region Удаление роли
        public async Task<ActionResult> Delete([Required]string id)
        {
            if (ModelState.IsValid)
            {
                Role role = await RoleManager.FindByIdAsync(id);

                if (role != null)
                {
                    IdentityResult res = await RoleManager.DeleteAsync(role);

                    if (res.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("Error", new string[] { "Не удалось удаить роль" });
                    }
                }
                else
                {
                    return View("Error", new string[] { "Роль не найдена" });
                }
            }
            else
            {
                return View("Error", new string[] { "Произошла ошибка" });
            }
        }
        #endregion

        #region Редактирование роли
        public async Task<ActionResult> Edit(string id)
        {
            Role role = await RoleManager.FindByIdAsync(id);

            string[] memberIds = role.Users.Select(x => x.UserId).ToArray();

            IEnumerable<User> members = await UserManager.Users.Where(u => memberIds.Any(y => y == u.Id)).ToListAsync();

            IEnumerable<User> nonMembers = await UserManager.Users.Except(members).ToListAsync();

            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleEditPostModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (string UserId in model.MemberIdsToAdd ?? new string[] { })
                {
                    IdentityResult res = await UserManager.AddToRoleAsync(UserId, model.RoleName);

                    if (!res.Succeeded)
                    {
                        AddErrorsToState(res);
                    }
                }

                foreach (string UserId in model.MemberIdsToDelete ?? new string[] { })
                {
                    IdentityResult res = await UserManager.RemoveFromRoleAsync(UserId, model.RoleName);

                    if (!res.Succeeded)
                    {
                        AddErrorsToState(res);
                    }
                }

                return RedirectToAction("Index");
            }

            return View("Error", new string[] { "Произошла ошибка" });
        }
        #endregion

        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        private void AddErrorsToState(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}