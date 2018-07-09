using Common.Entities.Enums;
using System;
using System.Collections.Generic;

namespace Common.Entities.Models
{
    public sealed class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public IdentificationType IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public Address Address { get; set; }
        public string Email { get; set; }

        public ICollection<ContactInformation> ContactInformations { get; set; }
    }
}
