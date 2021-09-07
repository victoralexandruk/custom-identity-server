using System;
using System.Collections.Generic;

namespace Identity.Models
{
    public class Role
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public IEnumerable<RolePermission> Permissions { get; set; } = new List<RolePermission>();
    }
}
