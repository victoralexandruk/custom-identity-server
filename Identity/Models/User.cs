using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Identity.Models
{
    public class User : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; }

        public bool Active { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        [PersonalData]
        public string SubjectId
        {
            get
            {
                return Id;
            }
        }
    }
}
