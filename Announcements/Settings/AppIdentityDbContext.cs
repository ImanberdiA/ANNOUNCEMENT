using Announcements.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Announcements.Settings
{
    public class AppIdentityDbContext: IdentityDbContext
    {
        public AppIdentityDbContext() : base("AnnConnection") { }

        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }

        public DbSet<Announ> Announs { get; set; }

    }
}