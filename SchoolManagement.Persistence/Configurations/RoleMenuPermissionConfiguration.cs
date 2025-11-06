using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence.Configurations
{
    public class RoleMenuPermissionConfiguration : IEntityTypeConfiguration<RoleMenuPermission>
    {
        public void Configure(EntityTypeBuilder<RoleMenuPermission> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Role)
                  .WithMany(r => r.RoleMenuPermissions)
                  .HasForeignKey(e => e.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Menu)
                  .WithMany(m => m.RoleMenuPermissions)
                  .HasForeignKey(e => e.MenuId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.RoleId, e.MenuId }).IsUnique();

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        }
    }
}
