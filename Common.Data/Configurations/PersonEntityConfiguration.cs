using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Entities.Models;

namespace Common.Data.Configurations
{
    public class PersonEntityConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasOne(p => p.Address)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.DateOfBirth)
                .IsRequired();

            builder.Property(p => p.Gender)
                .IsRequired();

            builder.Property(p => p.IdentificationType)
                .IsRequired();

            builder.Property(p => p.IdentificationNumber)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(p => p.MaritalStatus)
                .IsRequired();

            builder.Property(p => p.Nationality)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
