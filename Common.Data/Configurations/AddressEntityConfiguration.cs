using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Entities.Models;

namespace Common.Data.Configurations
{
    public class AddressEntityConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasOne(d => d.District)
                .WithMany()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Municipality)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Province)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
