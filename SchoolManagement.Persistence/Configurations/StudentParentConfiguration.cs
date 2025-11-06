using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence.Configurations
{
    public class StudentParentConfiguration : IEntityTypeConfiguration<StudentParent>
    {
        public void Configure(EntityTypeBuilder<StudentParent> entity)
        {
            entity.HasKey(sp => sp.Id);

            entity.Property(sp => sp.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(sp => sp.LastName).IsRequired().HasMaxLength(50);
            entity.Property(sp => sp.Email).HasMaxLength(100);
            entity.Property(sp => sp.Phone).HasMaxLength(20);
            entity.Property(sp => sp.Occupation).HasMaxLength(100);

            entity.HasOne(sp => sp.Student)
                  .WithMany(s => s.StudentParents)
                  .HasForeignKey(sp => sp.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.OwnsOne(sp => sp.Address, a =>
            {
                a.ToTable("StudentParentAddresses");
                a.Property(p => p.Street).HasMaxLength(200);
                a.Property(p => p.City).HasMaxLength(50);
                a.Property(p => p.State).HasMaxLength(50);
                a.Property(p => p.Country).HasMaxLength(50);
                a.Property(p => p.ZipCode).HasMaxLength(10);
            });

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        }
    }
}
