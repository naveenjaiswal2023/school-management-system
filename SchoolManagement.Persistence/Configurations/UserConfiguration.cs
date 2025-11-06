using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Username)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.Email)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.FirstName)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.LastName)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.PasswordHash)
                  .IsRequired()
                  .HasMaxLength(500);

            entity.Property(e => e.PhoneNumber)
                  .HasMaxLength(20);

            entity.HasOne(e => e.Student)
                  .WithOne()
                  .HasForeignKey<User>(e => e.StudentId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Employee)
                  .WithOne()
                  .HasForeignKey<User>(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        }
    }
}
