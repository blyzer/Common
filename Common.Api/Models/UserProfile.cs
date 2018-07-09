using System;
using System.Collections.Generic;
using Common.Entities.Enums;

namespace Common.Api.Models
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
