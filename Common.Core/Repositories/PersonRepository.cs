using Microsoft.EntityFrameworkCore;
using Common.Data;
using Common.Entities.Models;
using Common.Entities.Enums;
using System.Threading.Tasks;

namespace Common.Core.Repositories
{
    public class PersonRepository : RepositoryBase<Person>
    {
        private readonly DataContext _context;

        public PersonRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Person> GetByIdentificationNumberAsync(IdentificationType type, string number)
        {
            return await _context.Persons
                .Include(p => p.ContactInformations)
                .Include(p => p.Address)
                .FirstOrDefaultAsync(p => p.IdentificationType == type && p.IdentificationNumber == number);
        }
    }
}
