using Common360.Data;
using Common360.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace Common360.Core.Repositories
{
    public class ContactInformationRepository : RepositoryBase<ContactInformation>
    {
        private readonly DataContext _context;

        public ContactInformationRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public void RemoveAllByPersonId(int personId)
        {
            _context.ContactInformations.RemoveRange(_context.ContactInformations.Where(c => c.PersonId == personId));
        }

        public void AddPersonContacts(int personPersonId, ICollection<ContactInformation> personContactInformations)
        {
            personContactInformations.ToList().ForEach(c => c.PersonId = personPersonId);
            _context.ContactInformations.AddRange(personContactInformations);
        }
    }
}
