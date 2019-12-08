using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Announcements.Models
{
    public class Announ
    {
        public int Id { get; set; }

        [Required]
        public string AnnTitle { get; set; }

        [Required]
        public string Description { get; set; }

        public string Salary { get; set; }

        public int? UserId { get; set; }

        public virtual User User { get; set; }
    }
}