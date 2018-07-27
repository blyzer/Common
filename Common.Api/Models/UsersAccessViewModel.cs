using Common360.Entities.Enums;
using System.Collections.Generic;

namespace Common360.Api.Models
{
    public class UsersAccessViewModel
    {
        public string UserName { get; set; }
        public ICollection<AccessLevels> Access { get; set; }
    }
}