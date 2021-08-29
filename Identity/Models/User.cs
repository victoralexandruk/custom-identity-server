﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Identity.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }

        public bool Active { get; set; }

        public string Password { get; set; }

        public string SubjectId
        {
            get
            {
                return Id;
            }
        }

        public ICollection<IdentityUserRole<string>> Roles { get; set; }
    }
}
