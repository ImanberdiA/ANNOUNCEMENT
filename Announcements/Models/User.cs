using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Announcements.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<Announ> Announs { get; set; }

        public User()
        {
            Announs = new List<Announ>();
        }
    }
}