using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Announcements.Models
{
    public class RoleEditModel
    {
        [Required]
        public Role Role { get; set; }

        [Required]
        public IEnumerable<User> Members { get; set; }

        [Required]
        public IEnumerable<User> NonMembers { get; set; }
    }
}