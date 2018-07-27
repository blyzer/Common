using Common360.Core.Repositories;
using Common360.Entities.Models;
using Common360.Entities.Enums;
using System.Threading.Tasks;

namespace Common360.Core.Rules
{
    public class PersonRules : RulesBase<Person>
    {
        readonly PersonRepository _personRepository;

        public PersonRules(PersonRepository personRepository) : base(personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<Person> GetByIdentificationNumberAsync(IdentificationType type, string number)
            => await _personRepository.GetByIdentificationNumberAsync(type, number);
    }
}
