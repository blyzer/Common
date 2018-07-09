using Common.Entities.Enums;
using System.Collections.Generic;

namespace Common.Api.Models
{
    public class UsersAccessViewModel
    {
        public string UserName { get; set; }
        public ICollection<AccessLevels> Access { get; set; }
    }
}