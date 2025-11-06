using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence.Configurations
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> entity)
        {
            entity.HasOne(sec => sec.Class)
                  .WithMany(cls => cls.Sections)
                  .HasForeignKey(sec => sec.ClassId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        }
    }
}
