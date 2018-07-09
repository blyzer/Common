using System.Linq;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> GetAll();
        Task<T> GetAsync (params object[] keyValues);
        void AddAsync(T value);
        void UpdateAsync(T value);
        void Delete(T value);
        Task SaveChangesAsync();
    }
}
