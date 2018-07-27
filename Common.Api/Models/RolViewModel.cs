using System.Collections.Generic;

namespace Common360.Api.Models
{
    public class RolViewModel
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public ICollection<string> Access { get; set; }
    }
}