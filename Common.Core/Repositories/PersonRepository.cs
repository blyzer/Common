using Microsoft.EntityFrameworkCore;
using Common360.Data;
using Common360.Entities.Models;
using Common360.Entities.Enums;
using System.Threading.Tasks;

namespace Common360.Core.Repositories
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
