using System.Collections.Generic;

namespace Common.Entities.Models
{
    public class Province
    {
        public int ProvinceId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Municipality> Municipalities { get; set; }

    }
}
