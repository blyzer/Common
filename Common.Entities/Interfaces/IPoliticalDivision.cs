using Common360.Entities.Models;

namespace Common360.Entities.Interfaces
{
    public interface IPoliticalDivision
    {
        Province Province { get; set; }
        Municipality Municipality { get; set; }
        District District { get; set; }
        int ProvinceId { get; set; }
        int MunicipalityId { get; set; }
        int? DistrictId { get; set; }
    }
}
