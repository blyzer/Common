using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Core.Repositories;
using Common.Contracts;

namespace Common.Core.Rules
{
    public abstract class RulesBase<T> : IRulesBase<T> where T : class
    {
        private readonly RepositoryBase<T> _repositoryBase;

        protected RulesBase(RepositoryBase<T> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public virtual ICollection<T> GetAll()
            => _repositoryBase.GetAll().ToList();

        public virtual ICollection<T> GetAllPaged(int pageSize, int pageNumber)
        {
            return _repositoryBase.GetAll()
                .Take(pageSize).Skip(pageSize * pageNumber).ToList();
        }

        public virtual async Task<T> GetAsync(params object[] keyValues)
            => await _repositoryBase.GetAsync(keyValues);

        public virtual async Task<T> AddAsync(T newgeneric)
        {
            _repositoryBase.AddAsync(newgeneric);
            await _repositoryBase.SaveChangesAsync();
            return newgeneric;
        }

        public virtual async Task UpdateAsync(T newValue)
        {
            _repositoryBase.UpdateAsync(newValue);
            await _repositoryBase.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T value)
        {
            _repositoryBase.Delete(value);
            await _repositoryBase.SaveChangesAsync();
        }
    }
}
