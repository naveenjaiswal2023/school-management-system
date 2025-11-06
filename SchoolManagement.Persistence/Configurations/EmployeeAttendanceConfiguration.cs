using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence.Configurations
{
    public class EmployeeAttendanceConfiguration : IEntityTypeConfiguration<EmployeeAttendance>
    {
        public void Configure(EntityTypeBuilder<EmployeeAttendance> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Employee)
                  .WithMany(emp => emp.Attendances)
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.RegularHours).HasColumnType("decimal(5,2)");
            entity.Property(e => e.OvertimeHours).HasColumnType("decimal(5,2)");

            entity.HasIndex(e => new { e.EmployeeId, e.Date }).IsUnique();

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        }
    }
}
