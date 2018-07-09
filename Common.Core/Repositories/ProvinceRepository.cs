using Common.Data;
using Common.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Common.Core.Repositories
{
    public class ProvinceRepository : RepositoryBase<Province>
    {
        private readonly DataContext _context;

        public ProvinceRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<Province> GetAll() {
            return _context.Provinces
                .Include(p => p.Municipalities)
                .ThenInclude(m => m.Districts);
        }

        public virtual IQueryable<District> GetDistrictsByMunicipalityName(string provinceName, string municipalityName)
        {
            return _context.Provinces
                .Include(p => p.Municipalities)
                .ThenInclude(m => m.Districts)
                .Where(p => p.Name == provinceName)
                .SelectMany(p => p.Municipalities.Where(m => m.Name == municipalityName)
                        .SelectMany(m => m.Districts)
                );
        }
    }
}
