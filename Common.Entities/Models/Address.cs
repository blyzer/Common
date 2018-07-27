using Common360.Entities.Interfaces;

namespace Common360.Entities.Models
{
    public class Address : IPoliticalDivision
    {
        public int AddressId { get; set; }
        public int ProvinceId { get; set; }
        public int MunicipalityId { get; set; }
        public int? DistrictId { get; set; }
        public string Sector { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }

        public Province Province { get; set; }
        public Municipality Municipality { get; set; }
        public District District { get; set; }
    }
}
