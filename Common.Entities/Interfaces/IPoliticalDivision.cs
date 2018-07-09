using Common.Entities.Models;

namespace Common.Entities.Interfaces
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
