using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence.Configurations
{
    public class FeePaymentConfiguration : IEntityTypeConfiguration<FeePayment>
    {
        public void Configure(EntityTypeBuilder<FeePayment> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(fp => fp.Student)
                  .WithMany(s => s.FeePayments)
                  .HasForeignKey(fp => fp.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        }
    }
}
