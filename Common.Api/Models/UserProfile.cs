using System;
using System.Collections.Generic;
using Common360.Entities.Enums;

namespace Common360.Api.Models
{
    public class UserProfile
    {
        public string UserName { get; set; }
        public UserType UserType {get;set;}
        public string Token { get; set; }
        public DateTime TokenExpirationDate { get; set; }
        public ICollection<string> AccessList { get; set; }
    }
}
