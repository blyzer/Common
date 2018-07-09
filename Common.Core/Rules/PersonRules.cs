using Common.Core.Repositories;
using Common.Entities.Models;
using Common.Entities.Enums;
using System.Threading.Tasks;

namespace Common.Core.Rules
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
