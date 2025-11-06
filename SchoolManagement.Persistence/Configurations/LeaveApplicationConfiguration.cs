using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence.Configurations
{
    public class LeaveApplicationConfiguration : IEntityTypeConfiguration<LeaveApplication>
    {
        public void Configure(EntityTypeBuilder<LeaveApplication> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Employee)
                  .WithMany(emp => emp.LeaveApplications)
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ApprovedByEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.ApprovedBy)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.ApprovalRemarks).HasMaxLength(500);
            entity.Property(e => e.DocumentPath).HasMaxLength(250);

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        }
    }
}
