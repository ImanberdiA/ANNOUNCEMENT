using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Announcements.Models
{
    public class RoleEditPostModel
    {
        [Required]
        public string RoleName { get; set; }

        public string[] MemberIdsToAdd { get; set; }

        public string[] MemberIdsToDelete { get; set; }


    }
}