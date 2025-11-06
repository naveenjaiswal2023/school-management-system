using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence.Configurations
{
    public class PerformanceReviewConfiguration : IEntityTypeConfiguration<PerformanceReview>
    {
        public void Configure(EntityTypeBuilder<PerformanceReview> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Employee)
                  .WithMany(emp => emp.PerformanceReviews)
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Reviewer)
                  .WithMany()
                  .HasForeignKey(e => e.ReviewerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Goals).HasMaxLength(1000);
            entity.Property(e => e.Achievements).HasMaxLength(1000);
            entity.Property(e => e.AreasOfImprovement).HasMaxLength(1000);
            entity.Property(e => e.ReviewerComments).HasMaxLength(1000);
            entity.Property(e => e.EmployeeComments).HasMaxLength(1000);

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        }
    }
}
