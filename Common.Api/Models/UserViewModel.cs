using System.ComponentModel.DataAnnotations;

namespace Common360.Api.Models
{
    public class UserViewModel : PersonViewModel
    {
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SchoolCode { get; set; }
        [Required]
        public string RoleName { get; set; }
        [Required]
        public bool Active { get; set; }
    }
}
