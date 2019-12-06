using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Announcements.Models
{
    public class Announ
    {
        public int Id { get; set; }

        public string PosName { get; set; }

        public string Description { get; set; }

        public string Salary { get; set; }

        public int? UserId { get; set; }

        public User User { get; set; }
    }
}