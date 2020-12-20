using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AFI.Registration.Data.TypeConfiguration
{
    public class CustomerTypeConfiguration : IEntityTypeConfiguration<Entities.Customer>
    {
        public void Configure(EntityTypeBuilder<Entities.Customer> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(p => p.LastName).HasMaxLength(50).IsRequired();
            builder.Property(p => p.ReferenceNumber).HasMaxLength(9).IsRequired();
            builder.Property(p => p.EmailAddress).HasMaxLength(320);
        }
    }
}
