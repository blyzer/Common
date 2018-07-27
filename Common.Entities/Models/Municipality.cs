using System.Collections.Generic;

namespace Common360.Entities.Models
{
    public class Municipality
    {
        public int MunicipalityId { get; set; }

        public int ProvinceId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<District> Districts { get; set; }
    }
}
