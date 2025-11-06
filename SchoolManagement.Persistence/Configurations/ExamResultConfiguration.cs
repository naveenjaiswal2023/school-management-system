using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence.Configurations
{
    public class ExamResultConfiguration : IEntityTypeConfiguration<ExamResult>
    {
        public void Configure(EntityTypeBuilder<ExamResult> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(er => er.Student)
                  .WithMany(s => s.ExamResults)
                  .HasForeignKey(er => er.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        }
    }
}
