using Announcements.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Announcements.Settings
{
    public class AppRoleManager: RoleManager<Role>
    {
        public AppRoleManager(RoleStore<Role> store) : base(store) { }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
        {
            AppIdentityDbContext db = context.Get<AppIdentityDbContext>();
            AppRoleManager manager = new AppRoleManager(new RoleStore<Role>(db));
            return manager;
        }
    }
}