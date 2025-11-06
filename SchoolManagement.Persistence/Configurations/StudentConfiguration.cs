using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.StudentId).IsRequired().HasMaxLength(20);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);

            entity.OwnsOne(e => e.Address, a =>
            {
                a.ToTable("StudentAddresses");
                a.Property(p => p.Street).HasMaxLength(200);
                a.Property(p => p.City).HasMaxLength(50);
                a.Property(p => p.State).HasMaxLength(50);
                a.Property(p => p.Country).HasMaxLength(50);
                a.Property(p => p.ZipCode).HasMaxLength(10);
            });

            entity.OwnsOne(e => e.BiometricInfo, b =>
            {
                b.ToTable("StudentBiometricInfos");
                b.Property(p => p.DeviceId).HasMaxLength(50);
                b.Property(p => p.TemplateHash).HasMaxLength(500);
            });

            entity.HasOne(s => s.Class)
                  .WithMany(c => c.Students)
                  .HasForeignKey(s => s.ClassId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Section)
                  .WithMany(sec => sec.Students)
                  .HasForeignKey(s => s.SectionId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Email).IsUnique();

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        }
    }
}
