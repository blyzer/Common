using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public interface IRulesBase<T>
    {
        ICollection<T> GetAll();
        ICollection<T> GetAllPaged(int pageSize, int pageNumber);
        Task<T> GetAsync(params object[] keyValues);
        Task<T> AddAsync(T newgeneric);
        Task UpdateAsync(T newValue);
        Task DeleteAsync(T value);
    }
}
