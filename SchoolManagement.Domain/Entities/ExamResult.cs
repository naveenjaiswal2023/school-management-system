using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class ExamResult : BaseEntity
    {
        public Guid StudentId { get; private set; }
        public Guid ExamId { get; private set; }
        public Guid SubjectId { get; private set; }
        public decimal MarksObtained { get; private set; }
        public decimal MaxMarks { get; private set; }
        public string Grade { get; private set; }
        public decimal Percentage { get; private set; }
        public ResultStatus Status { get; private set; }
        public string Remarks { get; private set; }

        // Navigation Properties
        public virtual Student Student { get; private set; }

        private ExamResult() { }

        public ExamResult(Guid studentId, Guid examId, Guid subjectId,
                         decimal marksObtained, decimal maxMarks)
        {
            StudentId = studentId;
            ExamId = examId;
            SubjectId = subjectId;
            MarksObtained = marksObtained;
            MaxMarks = maxMarks;
            Status = ResultStatus.Published;

            CalculatePercentageAndGrade();
        }

        public void UpdateMarks(decimal marksObtained, decimal maxMarks)
        {
            MarksObtained = marksObtained;
            MaxMarks = maxMarks;
            CalculatePercentageAndGrade();
        }

        private void CalculatePercentageAndGrade()
        {
            Percentage = MaxMarks > 0 ? (MarksObtained / MaxMarks) * 100 : 0;
            Grade = Percentage switch
            {
                >= 90 => "A+",
                >= 80 => "A",
                >= 70 => "B+",
                >= 60 => "B",
                >= 50 => "C+",
                >= 40 => "C",
                >= 33 => "D",
                _ => "F"
            };
        }
    }
}
