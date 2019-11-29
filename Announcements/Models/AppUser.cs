using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Announcements.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<Announ> Announs { get; set; }

        public AppUser()
        {
            Announs = new List<Announ>();
        }
    }
}