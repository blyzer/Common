using Common360.Entities.Enums;

namespace Common360.Entities.Models
{
    public class ContactInformation
    {
        public int ContactInformationId { get; set; }
        public int PersonId { get; set; }
        public ContactType ContactType { get; set; }
        public string ContactValue { get; set; }
    }
}
