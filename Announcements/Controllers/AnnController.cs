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
    #region Класс управления объявлениями пользователя
    [Authorize]
    public class AnnController : Controller
    {
        AppIdentityDbContext db = new AppIdentityDbContext();

        #region Получить все объявления пользователя
        public async Task<ActionResult> GetUserAnns()
        {
            User user = await Manager.FindByNameAsync(HttpContext.User.Identity.Name);
            return View(user);
        }
        #endregion

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

                return RedirectToAction("GetUserAnns");
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }

            return View(ann);
        }
        #endregion

        #region Удаление объявления
        public async Task<ActionResult> Delete([Required]string id)
        {
            if (ModelState.IsValid)
            {
                Announ ann = await db.Announs.Where(a => a.Id.ToString() == id).FirstOrDefaultAsync();

                if (ann != null)
                {
                    db.Announs.Remove(ann);
                    await db.SaveChangesAsync();

                    return RedirectToAction("GetUserAnns");
                }
                else
                {
                    return View("Error", new string[] { "Не удалось найти объявление" });
                }
            }
            else
            {
                return View("Error", new string[] { "Произошла ошибка" });
            }
        }

        #endregion

        #region Редактирование объявления
        public async Task<ActionResult> Edit(string id)
        {
            Announ ann = await db.Announs.FindAsync(Int32.Parse(id));

            if (ann != null)
            {
                return View(ann);
            }
            else
            {
                return View("Error", new string[] { "Объявление не найдено" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Announ ann)
        {
            db.Entry(ann).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("GetUserAnns");
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

    #endregion
}