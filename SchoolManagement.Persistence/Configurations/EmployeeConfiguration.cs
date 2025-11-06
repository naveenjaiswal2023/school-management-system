using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);

            entity.OwnsOne(e => e.Address, a =>
            {
                a.ToTable("EmployeeAddresses");
                a.Property(p => p.Street).HasMaxLength(200);
                a.Property(p => p.City).HasMaxLength(50);
                a.Property(p => p.State).HasMaxLength(50);
                a.Property(p => p.Country).HasMaxLength(50);
                a.Property(p => p.ZipCode).HasMaxLength(10);
            });

            entity.OwnsOne(e => e.SalaryInfo, s =>
            {
                s.ToTable("EmployeeSalaryInfos");
                s.Property(p => p.BasicSalary).HasColumnType("decimal(18,2)");
                s.Property(p => p.HRA).HasColumnType("decimal(18,2)");
                s.Property(p => p.SpecialAllowance).HasColumnType("decimal(18,2)");
            });

            entity.OwnsOne(e => e.BiometricInfo, b =>
            {
                b.ToTable("EmployeeBiometricInfos");
                b.Property(p => p.DeviceId).HasMaxLength(50);
                b.Property(p => p.TemplateHash).HasMaxLength(500);
            });

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        }
    }
}
