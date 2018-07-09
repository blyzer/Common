using Microsoft.AspNetCore.Identity;
using Common.Entities.Enums;

namespace Common.Entities.Models
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }

        public UserType UserType { get; set; }
        public virtual Role Role { get; set; }
    }
}
