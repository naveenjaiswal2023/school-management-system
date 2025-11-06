using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> entity)
        {
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Code).IsRequired().HasMaxLength(50);

            entity.HasMany(d => d.Employees)
                  .WithOne(e => e.Department)
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.HeadOfDepartment)
                  .WithMany()
                  .HasForeignKey(d => d.HeadOfDepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        }
    }
}
