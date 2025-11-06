using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> entity)
        {
            entity.HasKey(rt => rt.Id);

            entity.Property(rt => rt.Token)
                  .IsRequired()
                  .HasMaxLength(500);

            entity.Property(rt => rt.ExpiryDate)
                  .IsRequired();

            entity.Property(rt => rt.IsRevoked)
                  .IsRequired();

            entity.HasOne(rt => rt.User)
                  .WithMany(u => u.RefreshTokens)
                  .HasForeignKey(rt => rt.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Property(rt => rt.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        }
    }
}
