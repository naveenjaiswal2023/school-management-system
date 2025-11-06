using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence.Configurations
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> entity)
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

            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.Route).HasMaxLength(200);
            entity.Property(e => e.Component).HasMaxLength(100);

            entity.HasOne(e => e.ParentMenu)
                  .WithMany(e => e.SubMenus)
                  .HasForeignKey(e => e.ParentMenuId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => new { e.ParentMenuId, e.SortOrder });

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        }
    }
}
