using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Common360.Entities.Models
{
    public class Role : IdentityRole<int>
    {
        public bool Active { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
