using System;
using System.Linq;
using System.Threading.Tasks;
using Common360.Entities.Models;
using Common360.Entities.Interfaces;
using Common360.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Common360.Core.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly DbContext _context;


        protected RepositoryBase(DbContext context)
        {
            _context = context;
        }

        public virtual int Count() => _context.Set<T>().Count();

        public virtual IQueryable<T> GetAll()
            => _context.Set<T>();

        public virtual async Task<T> GetAsync(params object[] keyValues)
            => await _context.Set<T>().FindAsync(keyValues);

        public virtual void AddAsync(T value)
            => _context.Set<T>().Add(value);

        public virtual void UpdateAsync(T value)
        {
        }

        public virtual void Delete(T value)
            => _context.Set<T>().Remove(value);

        public virtual async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

        protected void NormalizePoliticalDivision<TPoliticalDivision>(TPoliticalDivision value)
            where TPoliticalDivision : IPoliticalDivision
        {
            if (value.Province == null || value.District == null || value.Municipality == null)
            {
                throw new ArgumentException();
            }

            Province province = _context.Set<Province>()
                .Include(p => p.Municipalities)
                .ThenInclude(m => m.Districts)
                .FirstOrDefault(p => p.Name == value.Province.Name);

            if (province == null)
            {
                throw new ArgumentException("Invalid province");
            }

            value.ProvinceId = province.ProvinceId;
            value.Province = null;

            Municipality municipality =
                province.Municipalities.FirstOrDefault(m => m.Name == value.Municipality.Name);

            if (municipality == null)
            {
                throw new ArgumentException("Invalid municipality");
            }

            value.MunicipalityId = municipality.MunicipalityId;
            value.Municipality = null;

            District district = municipality.Districts.FirstOrDefault(d => d.Name == value.District?.Name);

            value.DistrictId = district?.DistrictId;
            value.District = null;
        }
    }
}
