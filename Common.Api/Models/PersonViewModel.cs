using Common360.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Common360.Api.Models
{
    public abstract class PersonViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public IdentificationType IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string Nationality { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public AddressViewModel Address { get; set; }
        public ICollection<ContactInfomationViewModel> ContactInformations { get; set; }
    }
}
