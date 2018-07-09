﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Common.Entities.Models
{
    public class Role : IdentityRole<int>
    {
        public bool Active { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
