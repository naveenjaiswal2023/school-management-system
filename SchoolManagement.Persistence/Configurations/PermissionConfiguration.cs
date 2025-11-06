using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.DisplayName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Description)
                  .HasMaxLength(500);

            entity.Property(e => e.Module)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.Action)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.Resource)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => new { e.Module, e.Action, e.Resource }).IsUnique();

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        }
    }
}
